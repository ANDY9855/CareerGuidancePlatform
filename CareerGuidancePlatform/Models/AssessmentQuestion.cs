namespace CareerGuidancePlatform.Models
{
    public class AssessmentQuestion
    {
        public int Id { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty; // Skills, Interests, Values, Personality
        public string OptionA { get; set; } = string.Empty;
        public string OptionB { get; set; } = string.Empty;
        public string OptionC { get; set; } = string.Empty;
        public string OptionD { get; set; } = string.Empty;
        public int OrderIndex { get; set; }
    }

    public class AssessmentResult
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }
        public int TechScore { get; set; }
        public int CreativeScore { get; set; }
        public int AnalyticalScore { get; set; }
        public int LeadershipScore { get; set; }
        public string? SuggestedCareer { get; set; }
        public string? CareerCompatibilityDetails { get; set; }
        public DateTime TakenAt { get; set; } = DateTime.UtcNow;
    }
}
