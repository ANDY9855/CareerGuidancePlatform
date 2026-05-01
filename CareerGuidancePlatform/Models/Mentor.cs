namespace CareerGuidancePlatform.Models
{
    public class Mentor
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public string Industry { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public string? ProfilePicture { get; set; }
        public string Email { get; set; } = string.Empty;
        public int YearsOfExperience { get; set; }
        public string Availability { get; set; } = string.Empty; // e.g. "Mon-Fri, 6PM-9PM"
        public double Rating { get; set; } = 5.0;
        public int TotalSessions { get; set; } = 0;
        public bool IsAvailable { get; set; } = true;
        public ICollection<MentorSession> Sessions { get; set; } = new List<MentorSession>();
    }

    public class MentorSession
    {
        public int Id { get; set; }
        public int MentorId { get; set; }
        public Mentor? Mentor { get; set; }
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }
        public DateTime ScheduledAt { get; set; }
        public string SessionType { get; set; } = string.Empty; // Video, Message, Group
        public string Topic { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending"; // Pending, Confirmed, Completed, Cancelled
        public string? Notes { get; set; }
    }
}
