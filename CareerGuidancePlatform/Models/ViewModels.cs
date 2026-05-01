namespace CareerGuidancePlatform.Models
{
    // Account ViewModels
    public class RegisterViewModel
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string? CurrentRole { get; set; }
        public string? Industry { get; set; }
    }

    public class LoginViewModel
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool RememberMe { get; set; }
    }

    // Assessment ViewModels
    public class AssessmentSubmitViewModel
    {
        public Dictionary<int, string> Answers { get; set; } = new();
    }

    // Goal ViewModels
    public class CreateGoalViewModel
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string TermType { get; set; } = "Short";
        public DateTime TargetDate { get; set; } = DateTime.Now.AddMonths(3);
        public List<string> Milestones { get; set; } = new();
    }

    // Mentor session booking
    public class BookSessionViewModel
    {
        public int MentorId { get; set; }
        public DateTime ScheduledAt { get; set; }
        public string SessionType { get; set; } = "Video";
        public string Topic { get; set; } = string.Empty;
    }
}
