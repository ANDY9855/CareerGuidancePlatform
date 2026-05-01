using Microsoft.AspNetCore.Identity;

namespace CareerGuidancePlatform.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public string? ProfilePicture { get; set; }
        public string? Bio { get; set; }
        public string? CurrentRole { get; set; }
        public string? Industry { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<AssessmentResult> AssessmentResults { get; set; } = new List<AssessmentResult>();
        public ICollection<Goal> Goals { get; set; } = new List<Goal>();
        public ICollection<JobApplication> JobApplications { get; set; } = new List<JobApplication>();
        public ICollection<MentorSession> MentorSessions { get; set; } = new List<MentorSession>();
        public ResumeData? ResumeData { get; set; }
    }
}
