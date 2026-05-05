namespace CareerGuidancePlatform.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string? Url { get; set; }
        public bool IsRead { get; set; } = false;
        public string Icon { get; set; } = "bi-bell";
        public string IconColor { get; set; } = "text-gradient";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
