namespace CareerGuidancePlatform.Models
{
    public class Resource
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // Course, Book, Article, Tutorial, Video
        public string Industry { get; set; } = string.Empty;
        public string SkillCategory { get; set; } = string.Empty;
        public string? Url { get; set; }
        public string? ThumbnailUrl { get; set; }
        public string Provider { get; set; } = string.Empty;
        public bool IsFree { get; set; } = true;
        public string? Duration { get; set; }
        public int DifficultyLevel { get; set; } = 1; // 1=Beginner, 2=Intermediate, 3=Advanced
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
