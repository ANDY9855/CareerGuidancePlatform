namespace CareerGuidancePlatform.Models
{
    public class CareerPath
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Industry { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string RequiredSkills { get; set; } = string.Empty;
        public string EducationRequired { get; set; } = string.Empty;
        public string AverageSalary { get; set; } = string.Empty;
        public string JobOutlook { get; set; } = string.Empty;
        public string IconClass { get; set; } = "bi-briefcase";
        public ICollection<CareerStage> Stages { get; set; } = new List<CareerStage>();
    }

    public class CareerStage
    {
        public int Id { get; set; }
        public int CareerPathId { get; set; }
        public CareerPath? CareerPath { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Skills { get; set; } = string.Empty;
        public int YearsRequired { get; set; }
        public int StageOrder { get; set; }
    }

    public class SuccessStory
    {
        public int Id { get; set; }
        public string PersonName { get; set; } = string.Empty;
        public string CurrentRole { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string Story { get; set; } = string.Empty;
        public string? ProfilePicture { get; set; }
        public string Industry { get; set; } = string.Empty;
        public DateTime PublishedAt { get; set; } = DateTime.UtcNow;
    }

    public class JobRoleOverview
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Industry { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string RequiredSkills { get; set; } = string.Empty;
        public string Responsibilities { get; set; } = string.Empty;
        public string SalaryRange { get; set; } = string.Empty;
        public string JobOutlook { get; set; } = string.Empty;
        public string IconClass { get; set; } = "bi-person-workspace";
    }
}
