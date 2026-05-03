namespace CareerGuidancePlatform.Models
{
    public class MentorMessage
    {
        public int Id { get; set; }
        public string UserId { get; set; } = "";
        public int MentorId { get; set; }
        public string Content { get; set; } = "";
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public bool IsFromMentor { get; set; } = false; // false = sent by user, true = sent by mentor
        public bool IsRead { get; set; } = false;

        public ApplicationUser? User { get; set; }
        public Mentor? Mentor { get; set; }
    }
}
