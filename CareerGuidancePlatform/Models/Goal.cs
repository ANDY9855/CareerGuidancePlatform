namespace CareerGuidancePlatform.Models
{
    public class Goal
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty; // Certification, Internship, Job, Skill, Networking
        public string TermType { get; set; } = "Short"; // Short, Long
        public int ProgressPercent { get; set; } = 0;
        public DateTime TargetDate { get; set; }
        public bool IsCompleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<GoalMilestone> Milestones { get; set; } = new List<GoalMilestone>();
    }

    public class GoalMilestone
    {
        public int Id { get; set; }
        public int GoalId { get; set; }
        public Goal? Goal { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; } = false;
        public int OrderIndex { get; set; }
    }
}
