using CareerGuidancePlatform.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CareerGuidancePlatform.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<AssessmentQuestion> AssessmentQuestions { get; set; }
        public DbSet<AssessmentResult> AssessmentResults { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Mentor> Mentors { get; set; }
        public DbSet<MentorSession> MentorSessions { get; set; }
        public DbSet<CareerPath> CareerPaths { get; set; }
        public DbSet<CareerStage> CareerStages { get; set; }
        public DbSet<SuccessStory> SuccessStories { get; set; }
        public DbSet<JobRoleOverview> JobRoleOverviews { get; set; }
        public DbSet<JobListing> JobListings { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }
        public DbSet<EmployerReview> EmployerReviews { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<GoalMilestone> GoalMilestones { get; set; }
        public DbSet<NetworkingEvent> NetworkingEvents { get; set; }
        public DbSet<PeerPost> PeerPosts { get; set; }
        public DbSet<ResumeData> ResumeData { get; set; }
        public DbSet<ResumeExperience> ResumeExperiences { get; set; }
        public DbSet<ResumeEducation> ResumeEducations { get; set; }
        public DbSet<ResumeCertification> ResumeCertifications { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AssessmentResult>()
                .HasOne(r => r.User)
                .WithMany(u => u.AssessmentResults)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Goal>()
                .HasOne(g => g.User)
                .WithMany(u => u.Goals)
                .HasForeignKey(g => g.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<JobApplication>()
                .HasOne(a => a.User)
                .WithMany(u => u.JobApplications)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<MentorSession>()
                .HasOne(s => s.User)
                .WithMany(u => u.MentorSessions)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ResumeData>()
                .HasOne(r => r.User)
                .WithOne(u => u.ResumeData)
                .HasForeignKey<ResumeData>(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
