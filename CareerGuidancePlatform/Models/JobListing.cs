namespace CareerGuidancePlatform.Models
{
    public class JobListing
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // Full-time, Part-time, Internship, Remote
        public string Industry { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Requirements { get; set; } = string.Empty;
        public string SalaryRange { get; set; } = string.Empty;
        public string? CompanyLogo { get; set; }
        public DateTime PostedAt { get; set; } = DateTime.UtcNow;
        public DateTime Deadline { get; set; }
        public bool IsActive { get; set; } = true;
        public ICollection<JobApplication> Applications { get; set; } = new List<JobApplication>();
        public ICollection<EmployerReview> Reviews { get; set; } = new List<EmployerReview>();
    }

    public class JobApplication
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }
        public int JobListingId { get; set; }
        public JobListing? JobListing { get; set; }
        public string Status { get; set; } = "Applied"; // Applied, Interview, Offer, Rejected
        public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
        public DateTime? FollowUpDate { get; set; }
        public string? Notes { get; set; }
    }

    public class EmployerReview
    {
        public int Id { get; set; }
        public int JobListingId { get; set; }
        public JobListing? JobListing { get; set; }
        public string ReviewerName { get; set; } = string.Empty;
        public int Rating { get; set; } // 1-5
        public string ReviewText { get; set; } = string.Empty;
        public DateTime ReviewedAt { get; set; } = DateTime.UtcNow;
    }
}
