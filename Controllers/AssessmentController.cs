using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.DTOs;
using WebApplication3.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using WebApplication3.Models;

[ApiController]
[Route("api/assessments")]
public class AssessmentsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AssessmentsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateAssessment([FromBody] CreateAssessmentDto dto)
    {
        if (dto == null || dto.QuestionsIds == null || !dto.QuestionsIds.Any())
        {
            return BadRequest(new { Message = "Invalid request. Ensure all required fields are provided." });
        }

        var questionIds = dto.QuestionsIds.Select(q => q.Id).ToList();
        var questions = await _context.Questions
            .Where(q => questionIds.Contains(q.Id))
            .ToListAsync();

        if (questions.Count != dto.QuestionsIds.Count)
        {
            return BadRequest(new { Message = "One or more questions not found in the database." });
        }

        var assessment = new Assessment
        {
            Name = dto.Name,
            Duration = TimeSpan.Parse(dto.Duration),
            AssessmentDate = DateTime.Parse(dto.AssessmentDate),
            StartTime = TimeSpan.Parse(dto.StartTime),
            EndTime = TimeSpan.Parse(dto.EndTime),
            TotalMark = dto.TotalMark,
            QuestionsCount = dto.QuestionsCount
        };

        _context.Assessments.Add(assessment);
        await _context.SaveChangesAsync();

        var assessmentQuestions = dto.QuestionsIds.Select(q => new AssessmentQuestion
        {
            AssessmentId = assessment.Id,
            QuestionId = q.Id,
            Mark = q.Mark
        }).ToList();

        _context.AssessmentQuestions.AddRange(assessmentQuestions);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Assessment created successfully", AssessmentId = assessment.Id });
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllAssessments()
    {
        var assessments = await _context.Assessments
            .Include(a => a.AssessmentQuestions)
            .ThenInclude(aq => aq.Question)
            .ToListAsync();

        var assessmentDtos = assessments.Select(a => new
        {
            a.Id,
            a.Name,
            Duration = a.Duration.ToString(),
            Time = a.AssessmentDate.ToString("yyyy-MM-dd HH:mm:ss"),
            StartTime = a.StartTime.ToString(),
            EndTime = a.EndTime.ToString(),
            a.TotalMark,
            a.QuestionsCount,
            Questions = a.AssessmentQuestions.Select(aq => new
            {
                aq.QuestionId,
                aq.Question.Prompt,
                aq.Mark
            })
        });

        return Ok(assessmentDtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAssessmentById(int id)
    {
        var assessment = await _context.Assessments
            .Include(a => a.AssessmentQuestions)
            .ThenInclude(aq => aq.Question)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (assessment == null)
        {
            return NotFound(new { Message = "Assessment not found." });
        }

        var assessmentDto = new
        {
            assessment.Id,
            assessment.Name,
            Duration = assessment.Duration.ToString(),
            Time = assessment.AssessmentDate.ToString("yyyy-MM-dd HH:mm:ss"),
            StartTime = assessment.StartTime.ToString(),
            EndTime = assessment.EndTime.ToString(),
            assessment.TotalMark,
            assessment.QuestionsCount,
            Questions = assessment.AssessmentQuestions.Select(aq => new
            {
                aq.QuestionId,
                aq.Question.Prompt,
                aq.Mark
            })
        };

        return Ok(assessmentDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAssessment(int id, [FromBody] UpdateAssessmentDto dto)
    {
        var assessment = await _context.Assessments
            .Include(a => a.AssessmentQuestions)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (assessment == null)
        {
            return NotFound(new { Message = "Assessment not found." });
        }

        if (dto == null || dto.QuestionsIds == null || !dto.QuestionsIds.Any())
        {
            return BadRequest(new { Message = "Invalid request. Ensure all required fields are provided." });
        }

        // Validate that all provided question IDs exist
        var providedQuestionIds = dto.QuestionsIds.Select(q => q.Id).ToList();
        var existingQuestionIds = await _context.Questions
            .Where(q => providedQuestionIds.Contains(q.Id))
            .Select(q => q.Id)
            .ToListAsync();

        var invalidIds = providedQuestionIds.Except(existingQuestionIds).ToList();
        if (invalidIds.Any())
        {
            return BadRequest(new { Message = $"Invalid Question IDs: {string.Join(", ", invalidIds)}" });
        }

        // Update basic information
        assessment.Name = dto.Name;
        assessment.Duration = TimeSpan.Parse(dto.Duration);
        assessment.AssessmentDate = DateTime.Parse(dto.Time);
        assessment.StartTime = TimeSpan.Parse(dto.StartTime);
        assessment.EndTime = TimeSpan.Parse(dto.EndTime);
        assessment.TotalMark = dto.TotalMark;
        assessment.QuestionsCount = dto.QuestionsCount;

        // Remove old questions and add new ones
        _context.AssessmentQuestions.RemoveRange(assessment.AssessmentQuestions);

        var newAssessmentQuestions = dto.QuestionsIds.Select(q => new AssessmentQuestion
        {
            AssessmentId = assessment.Id,
            QuestionId = q.Id,
            Mark = q.Mark
        }).ToList();

        await _context.AssessmentQuestions.AddRangeAsync(newAssessmentQuestions);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Assessment updated successfully." });
    }

}
