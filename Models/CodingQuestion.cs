namespace WebApplication3.Models
{
    public class CodingQuestion
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public int InputsCount { get; set; }
        public Question Question { get; set; }
        public List<TestCase>? TestCases { get; set; }
    }
}
