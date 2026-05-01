namespace CareerGuidancePlatform.Models
{
    public class NetworkingEvent
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string EventType { get; set; } = string.Empty; // Conference, Webinar, Meetup, Workshop
        public string Organizer { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty; // or "Online"
        public DateTime EventDate { get; set; }
        public string? RegistrationUrl { get; set; }
        public bool IsOnline { get; set; } = false;
        public string Industry { get; set; } = string.Empty;
        public string IconClass { get; set; } = "bi-calendar-event";
    }

    public class PeerPost
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }
        public string Content { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty; // Advice, Question, Experience, Opportunity
        public int Likes { get; set; } = 0;
        public DateTime PostedAt { get; set; } = DateTime.UtcNow;
    }
}
