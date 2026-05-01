namespace CareerGuidancePlatform.Models
{
    public class ResumeData
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string? LinkedIn { get; set; }
        public string? Portfolio { get; set; }
        public string Summary { get; set; } = string.Empty;
        public string Skills { get; set; } = string.Empty; // comma separated
        public string Template { get; set; } = "Modern"; // Modern, Classic, Creative
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        public ICollection<ResumeExperience> Experiences { get; set; } = new List<ResumeExperience>();
        public ICollection<ResumeEducation> Educations { get; set; } = new List<ResumeEducation>();
        public ICollection<ResumeCertification> Certifications { get; set; } = new List<ResumeCertification>();
    }

    public class ResumeExperience
    {
        public int Id { get; set; }
        public int ResumeDataId { get; set; }
        public ResumeData? ResumeData { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string StartDate { get; set; } = string.Empty;
        public string EndDate { get; set; } = "Present";
        public string Description { get; set; } = string.Empty;
    }

    public class ResumeEducation
    {
        public int Id { get; set; }
        public int ResumeDataId { get; set; }
        public ResumeData? ResumeData { get; set; }
        public string Degree { get; set; } = string.Empty;
        public string Institution { get; set; } = string.Empty;
        public string Year { get; set; } = string.Empty;
        public string? GPA { get; set; }
    }

    public class ResumeCertification
    {
        public int Id { get; set; }
        public int ResumeDataId { get; set; }
        public ResumeData? ResumeData { get; set; }
        public string Name { get; set; } = string.Empty;
        public string IssuedBy { get; set; } = string.Empty;
        public string Year { get; set; } = string.Empty;
    }
}
