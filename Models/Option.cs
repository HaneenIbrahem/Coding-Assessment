namespace WebApplication3.Models
{
    public class MCQOption
    {
        public int Id { get; set; }
        public int MCQQuestionId { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
    }
}