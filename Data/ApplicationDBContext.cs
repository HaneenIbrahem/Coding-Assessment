using Microsoft.EntityFrameworkCore;
using WebApplication3.Models;

namespace WebApplication3.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Question> Questions { get; set; }
        public DbSet<MCQQuestion> MCQQuestions { get; set; }
        public DbSet<CodingQuestion> CodingQuestions { get; set; }
        public DbSet<EssayQuestion> EssayQuestions { get; set; }
        public DbSet<Assessment> Assessments { get; set; }
        public DbSet<AssessmentQuestion> AssessmentQuestions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AssessmentQuestion>()
                .HasKey(aq => new { aq.AssessmentId, aq.QuestionId });

            modelBuilder.Entity<AssessmentQuestion>()
                .HasOne(aq => aq.Assessment)
                .WithMany(a => a.AssessmentQuestions)
                .HasForeignKey(aq => aq.AssessmentId);

            modelBuilder.Entity<AssessmentQuestion>()
                .HasOne(aq => aq.Question)
                .WithMany(q => q.AssessmentQuestions)
                .HasForeignKey(aq => aq.QuestionId);
        }
    }
}
