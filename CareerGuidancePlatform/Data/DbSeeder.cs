using CareerGuidancePlatform.Data;
using CareerGuidancePlatform.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CareerGuidancePlatform.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            var context = services.GetRequiredService<AppDbContext>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            await context.Database.MigrateAsync();

            // Seed Roles
            string[] roles = { "Admin", "User", "Mentor" };
            foreach (var role in roles)
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));

            // Seed Admin
            if (await userManager.FindByEmailAsync("admin@career.com") == null)
            {
                var admin = new ApplicationUser
                {
                    FullName = "Admin User",
                    UserName = "admin@career.com",
                    Email = "admin@career.com",
                    EmailConfirmed = true,
                    CurrentRole = "Platform Admin",
                    Industry = "Technology"
                };
                await userManager.CreateAsync(admin, "Admin@123");
                await userManager.AddToRoleAsync(admin, "Admin");
            }

            // Seed demo user
            if (await userManager.FindByEmailAsync("demo@career.com") == null)
            {
                var user = new ApplicationUser
                {
                    FullName = "Alex Johnson",
                    UserName = "demo@career.com",
                    Email = "demo@career.com",
                    EmailConfirmed = true,
                    CurrentRole = "Student",
                    Industry = "Technology"
                };
                await userManager.CreateAsync(user, "Demo@123");
                await userManager.AddToRoleAsync(user, "User");
            }

            // Seed Assessment Questions
            if (!context.AssessmentQuestions.Any())
            {
                context.AssessmentQuestions.AddRange(
                    new AssessmentQuestion { QuestionText = "Which activity do you enjoy most?", Category = "Interests", OptionA = "Writing code and building apps", OptionB = "Designing graphics and UI", OptionC = "Analyzing data and finding patterns", OptionD = "Leading teams and managing projects", OrderIndex = 1 },
                    new AssessmentQuestion { QuestionText = "How do you prefer to solve problems?", Category = "Personality", OptionA = "Systematically, step by step", OptionB = "Creatively, with out-of-the-box ideas", OptionC = "Using data and research", OptionD = "By collaborating with others", OrderIndex = 2 },
                    new AssessmentQuestion { QuestionText = "Which subject do you find most engaging?", Category = "Skills", OptionA = "Mathematics and Programming", OptionB = "Art and Communication", OptionC = "Statistics and Science", OptionD = "Business and Management", OrderIndex = 3 },
                    new AssessmentQuestion { QuestionText = "What kind of work environment suits you?", Category = "Values", OptionA = "Working independently on technical challenges", OptionB = "Creative studio with design freedom", OptionC = "Research lab with data and experiments", OptionD = "Dynamic team with leadership opportunities", OrderIndex = 4 },
                    new AssessmentQuestion { QuestionText = "Which career goal resonates most with you?", Category = "Interests", OptionA = "Build software used by millions", OptionB = "Create experiences people love", OptionC = "Discover insights from complex data", OptionD = "Build and lead a successful team", OrderIndex = 5 },
                    new AssessmentQuestion { QuestionText = "Your strongest natural ability is:", Category = "Skills", OptionA = "Logical and analytical thinking", OptionB = "Visual creativity and aesthetics", OptionC = "Research and critical evaluation", OptionD = "Communication and persuasion", OrderIndex = 6 },
                    new AssessmentQuestion { QuestionText = "In a group project, you typically:", Category = "Personality", OptionA = "Handle the technical implementation", OptionB = "Design the presentation and visuals", OptionC = "Research and fact-check everything", OptionD = "Coordinate the team and timeline", OrderIndex = 7 },
                    new AssessmentQuestion { QuestionText = "Which tool would you most enjoy mastering?", Category = "Skills", OptionA = "VS Code / IDEs and programming languages", OptionB = "Figma / Adobe tools", OptionC = "Tableau / Python for data", OptionD = "Jira / project management tools", OrderIndex = 8 }
                );
            }

            // Seed Mentors
            if (!context.Mentors.Any())
            {
                context.Mentors.AddRange(
                    new Mentor { Name = "Sarah Chen", Specialization = "Software Engineering", Industry = "Technology", Bio = "Senior Software Engineer at Google with 10+ years experience. Passionate about helping aspiring developers break into top tech companies.", Email = "sarah@mentor.com", YearsOfExperience = 10, Availability = "Weekends, 10AM-2PM", Rating = 4.9, TotalSessions = 87 },
                    new Mentor { Name = "Marcus Williams", Specialization = "Data Science & AI", Industry = "Technology", Bio = "Data Scientist at Microsoft. Specializes in ML/AI careers and helping candidates prepare for data science interviews.", Email = "marcus@mentor.com", YearsOfExperience = 8, Availability = "Mon, Wed, Fri 7PM-9PM", Rating = 4.8, TotalSessions = 64 },
                    new Mentor { Name = "Priya Patel", Specialization = "UX/UI Design", Industry = "Design", Bio = "Lead UX Designer at Meta. Helps designers build stunning portfolios and land roles at top product companies.", Email = "priya@mentor.com", YearsOfExperience = 7, Availability = "Tue, Thu 6PM-8PM", Rating = 4.9, TotalSessions = 52 },
                    new Mentor { Name = "James Torres", Specialization = "Product Management", Industry = "Technology", Bio = "VP of Product at a Series B startup. Guides aspiring PMs through interviews, case studies, and career transitions.", Email = "james@mentor.com", YearsOfExperience = 12, Availability = "Weekdays 8PM-10PM", Rating = 4.7, TotalSessions = 103 },
                    new Mentor { Name = "Aisha Rahman", Specialization = "Cybersecurity", Industry = "Security", Bio = "Cybersecurity Lead at Deloitte. CISSP certified. Mentors students entering the security field.", Email = "aisha@mentor.com", YearsOfExperience = 9, Availability = "Sat-Sun 9AM-12PM", Rating = 4.8, TotalSessions = 41 },
                    new Mentor { Name = "David Kim", Specialization = "Cloud Architecture", Industry = "Technology", Bio = "AWS Solutions Architect with certifications in all major cloud platforms. Helps engineers transition to cloud roles.", Email = "david@mentor.com", YearsOfExperience = 11, Availability = "Mon-Fri 7AM-9AM", Rating = 4.6, TotalSessions = 78 }
                );
            }

            // Seed Resources
            if (!context.Resources.Any())
            {
                context.Resources.AddRange(
                    new Resource { Title = "The Complete Web Developer Bootcamp", Description = "Master HTML, CSS, JavaScript, React, Node.js and more in this comprehensive course.", Type = "Course", Industry = "Technology", SkillCategory = "Web Development", Url = "https://www.udemy.com", Provider = "Udemy", IsFree = false, Duration = "65 hours", DifficultyLevel = 1 },
                    new Resource { Title = "CS50: Introduction to Computer Science", Description = "Harvard's famous intro to CS course. Perfect foundation for aspiring developers.", Type = "Course", Industry = "Technology", SkillCategory = "Programming", Url = "https://cs50.harvard.edu", Provider = "Harvard / edX", IsFree = true, Duration = "12 weeks", DifficultyLevel = 1 },
                    new Resource { Title = "Data Science with Python", Description = "Learn pandas, numpy, matplotlib, scikit-learn and build real ML projects.", Type = "Course", Industry = "Technology", SkillCategory = "Data Science", Url = "https://www.coursera.org", Provider = "Coursera", IsFree = false, Duration = "40 hours", DifficultyLevel = 2 },
                    new Resource { Title = "Google UX Design Certificate", Description = "Professional certificate from Google to build job-ready UX design skills.", Type = "Course", Industry = "Design", SkillCategory = "UX Design", Url = "https://www.coursera.org", Provider = "Google / Coursera", IsFree = false, Duration = "6 months", DifficultyLevel = 1 },
                    new Resource { Title = "AWS Cloud Practitioner Essentials", Description = "Foundational course for AWS Cloud services. Prepares for AWS certification.", Type = "Course", Industry = "Technology", SkillCategory = "Cloud Computing", Url = "https://aws.amazon.com/training", Provider = "Amazon AWS", IsFree = true, Duration = "6 hours", DifficultyLevel = 1 },
                    new Resource { Title = "Clean Code by Robert C. Martin", Description = "A handbook of agile software craftsmanship. Must-read for every developer.", Type = "Book", Industry = "Technology", SkillCategory = "Software Engineering", Provider = "O'Reilly", IsFree = false, DifficultyLevel = 2 },
                    new Resource { Title = "The Pragmatic Programmer", Description = "Your journey to mastery as a software developer. Timeless advice.", Type = "Book", Industry = "Technology", SkillCategory = "Programming", Provider = "Addison-Wesley", IsFree = false, DifficultyLevel = 2 },
                    new Resource { Title = "Introduction to Machine Learning", Description = "Beginner-friendly article series covering ML fundamentals with examples.", Type = "Article", Industry = "Technology", SkillCategory = "Machine Learning", Url = "https://www.towardsdatascience.com", Provider = "Towards Data Science", IsFree = true, DifficultyLevel = 2 },
                    new Resource { Title = "Cybersecurity for Everyone", Description = "Learn the fundamentals of cybersecurity: threats, cryptography, and network security.", Type = "Course", Industry = "Security", SkillCategory = "Cybersecurity", Url = "https://www.coursera.org", Provider = "University of Maryland / Coursera", IsFree = true, Duration = "4 weeks", DifficultyLevel = 1 },
                    new Resource { Title = "Agile Project Management Tutorial", Description = "Master Agile, Scrum and Kanban methodologies for modern project management.", Type = "Tutorial", Industry = "Management", SkillCategory = "Project Management", Url = "https://www.youtube.com", Provider = "YouTube", IsFree = true, Duration = "3 hours", DifficultyLevel = 1 }
                );
            }

            // Seed Career Paths
            if (!context.CareerPaths.Any())
            {
                var softwarePath = new CareerPath
                {
                    Title = "Software Engineer", Industry = "Technology",
                    Description = "Build software solutions that power businesses and consumers worldwide.",
                    RequiredSkills = "Programming, Data Structures, Algorithms, System Design",
                    EducationRequired = "Bachelor's in Computer Science or related field",
                    AverageSalary = "$85,000 - $180,000", JobOutlook = "22% growth (Much faster than average)",
                    IconClass = "bi-code-slash",
                    Stages = new List<CareerStage>
                    {
                        new CareerStage { Title = "Junior Developer", Description = "Write and test code under supervision", Skills = "HTML, CSS, JS, Python/Java basics", YearsRequired = 0, StageOrder = 1 },
                        new CareerStage { Title = "Mid-Level Developer", Description = "Own features independently, code reviews", Skills = "Frameworks, APIs, Git, Testing", YearsRequired = 2, StageOrder = 2 },
                        new CareerStage { Title = "Senior Developer", Description = "Lead technical decisions, mentor juniors", Skills = "System Design, Architecture, Leadership", YearsRequired = 5, StageOrder = 3 },
                        new CareerStage { Title = "Principal / Staff Engineer", Description = "Drive org-wide technical strategy", Skills = "Cross-team collaboration, Vision", YearsRequired = 10, StageOrder = 4 }
                    }
                };
                var dataPath = new CareerPath
                {
                    Title = "Data Scientist", Industry = "Technology",
                    Description = "Extract insights from complex data to drive business decisions.",
                    RequiredSkills = "Python, Statistics, ML, SQL, Data Visualization",
                    EducationRequired = "Bachelor's in Statistics, CS, or Mathematics",
                    AverageSalary = "$90,000 - $160,000", JobOutlook = "35% growth (Explosive growth)",
                    IconClass = "bi-bar-chart-fill",
                    Stages = new List<CareerStage>
                    {
                        new CareerStage { Title = "Data Analyst", Description = "Clean and analyze datasets, create reports", Skills = "SQL, Excel, Basic Python", YearsRequired = 0, StageOrder = 1 },
                        new CareerStage { Title = "Data Scientist", Description = "Build ML models and predictive analytics", Skills = "Python, Scikit-learn, TensorFlow", YearsRequired = 2, StageOrder = 2 },
                        new CareerStage { Title = "Senior Data Scientist", Description = "Lead ML projects and research", Skills = "Deep Learning, MLOps, Cloud AI", YearsRequired = 5, StageOrder = 3 },
                        new CareerStage { Title = "Chief Data Officer", Description = "Define data strategy for the organization", Skills = "Leadership, Strategy, Governance", YearsRequired = 10, StageOrder = 4 }
                    }
                };
                context.CareerPaths.AddRange(softwarePath, dataPath,
                    new CareerPath { Title = "UX Designer", Industry = "Design", Description = "Design digital experiences that delight users.", RequiredSkills = "Figma, User Research, Wireframing, Prototyping", EducationRequired = "Degree in Design, HCI, or Psychology", AverageSalary = "$70,000 - $140,000", JobOutlook = "13% growth", IconClass = "bi-palette" },
                    new CareerPath { Title = "Product Manager", Industry = "Technology", Description = "Bridge business, design and engineering to ship great products.", RequiredSkills = "Strategy, Communication, Analytics, Leadership", EducationRequired = "Bachelor's in any field + MBA advantageous", AverageSalary = "$100,000 - $200,000", JobOutlook = "10% growth", IconClass = "bi-kanban" },
                    new CareerPath { Title = "Cybersecurity Analyst", Industry = "Security", Description = "Protect organizations from digital threats and breaches.", RequiredSkills = "Networking, Ethical Hacking, SIEM, Cryptography", EducationRequired = "Bachelor's in CS or Cybersecurity + Certs", AverageSalary = "$80,000 - $150,000", JobOutlook = "32% growth", IconClass = "bi-shield-lock" }
                );
            }

            // Seed Success Stories
            if (!context.SuccessStories.Any())
            {
                context.SuccessStories.AddRange(
                    new SuccessStory { PersonName = "Rania Aziz", CurrentRole = "Software Engineer", Company = "Amazon", Story = "I started as a complete beginner with no CS degree. After using the Career Guidance Platform, I took the assessment, followed the recommended learning path, and connected with a mentor who helped me prepare for technical interviews. 8 months later, I landed a role at Amazon!", Industry = "Technology" },
                    new SuccessStory { PersonName = "Carlos Mendez", CurrentRole = "Data Scientist", Company = "Netflix", Story = "The career assessment pointed me towards data science when I was unsure between software engineering and analytics. The mentors here have industry-level insights that you can't find on YouTube. I got an offer from Netflix after 6 months of focused preparation.", Industry = "Technology" },
                    new SuccessStory { PersonName = "Zara Khan", CurrentRole = "UX Designer", Company = "Spotify", Story = "As someone transitioning from graphic design, I had the skills but didn't know how to position myself for tech UX roles. The mentor directory connected me with a designer at Meta who reviewed my portfolio and guided my transition. Now I'm at Spotify!", Industry = "Design" }
                );
            }

            // Seed Job Listings
            if (!context.JobListings.Any())
            {
                context.JobListings.AddRange(
                    new JobListing { Title = "Junior Software Engineer", Company = "TechCorp Solutions", Location = "Karachi, Pakistan", Type = "Full-time", Industry = "Technology", Description = "Join our growing engineering team to build scalable web applications using .NET and React.", Requirements = "1-2 years experience, C#, JavaScript, SQL knowledge", SalaryRange = "PKR 80,000 - 120,000/month", Deadline = DateTime.Now.AddDays(30) },
                    new JobListing { Title = "Frontend Developer Intern", Company = "Digital Nest", Location = "Lahore, Pakistan (Remote)", Type = "Internship", Industry = "Technology", Description = "3-month paid internship working on exciting frontend projects with React and TypeScript.", Requirements = "HTML, CSS, JavaScript, basic React", SalaryRange = "PKR 30,000 - 50,000/month", Deadline = DateTime.Now.AddDays(15) },
                    new JobListing { Title = "Data Analyst", Company = "Analytics Hub", Location = "Islamabad, Pakistan", Type = "Full-time", Industry = "Technology", Description = "Analyze business data and create dashboards to drive strategic decisions.", Requirements = "SQL, Power BI/Tableau, Python basics, Excel", SalaryRange = "PKR 90,000 - 140,000/month", Deadline = DateTime.Now.AddDays(21) },
                    new JobListing { Title = "UI/UX Designer", Company = "Creative Labs", Location = "Remote", Type = "Part-time", Industry = "Design", Description = "Design beautiful, user-centered interfaces for our suite of B2B products.", Requirements = "Figma, User Research, 1+ year portfolio", SalaryRange = "PKR 60,000 - 100,000/month", Deadline = DateTime.Now.AddDays(10) },
                    new JobListing { Title = "Cloud Engineer", Company = "CloudFirst Pakistan", Location = "Karachi, Pakistan", Type = "Full-time", Industry = "Technology", Description = "Design and manage cloud infrastructure on AWS and Azure.", Requirements = "AWS/Azure experience, Terraform, Docker, Linux", SalaryRange = "PKR 120,000 - 200,000/month", Deadline = DateTime.Now.AddDays(45) },
                    new JobListing { Title = "Cybersecurity Analyst Intern", Company = "SecureNet Systems", Location = "Karachi, Pakistan", Type = "Internship", Industry = "Security", Description = "Learn and apply cybersecurity best practices in a real enterprise environment.", Requirements = "Networking basics, keen interest in security", SalaryRange = "PKR 25,000 - 40,000/month", Deadline = DateTime.Now.AddDays(20) }
                );
            }

            // Seed Networking Events
            if (!context.NetworkingEvents.Any())
            {
                context.NetworkingEvents.AddRange(
                    new NetworkingEvent { Title = "Pakistan Tech Summit 2026", Description = "The largest technology conference in Pakistan. Meet industry leaders, attend workshops, and expand your network.", EventType = "Conference", Organizer = "Tech Pakistan", Location = "Karachi Expo Centre", EventDate = DateTime.Now.AddDays(45), IsOnline = false, Industry = "Technology", IconClass = "bi-building" },
                    new NetworkingEvent { Title = "Women in Tech Webinar", Description = "Empowering women to break barriers in the tech industry. Featuring speakers from Google, Microsoft, and Amazon.", EventType = "Webinar", Organizer = "WomenTech Network", Location = "Online (Zoom)", EventDate = DateTime.Now.AddDays(7), RegistrationUrl = "https://zoom.us", IsOnline = true, Industry = "Technology", IconClass = "bi-camera-video" },
                    new NetworkingEvent { Title = "Startup Career Fair 2026", Description = "Meet top startups looking to hire talented individuals. Bring your resume and portfolio.", EventType = "Meetup", Organizer = "Startup Grind Karachi", Location = "The Hub, Karachi", EventDate = DateTime.Now.AddDays(21), IsOnline = false, Industry = "General", IconClass = "bi-people" },
                    new NetworkingEvent { Title = "Data Science Workshop", Description = "Hands-on workshop covering Python for data analysis, visualization, and building your first ML model.", EventType = "Workshop", Organizer = "DataKarachi", Location = "Online (Google Meet)", EventDate = DateTime.Now.AddDays(14), RegistrationUrl = "https://meet.google.com", IsOnline = true, Industry = "Technology", IconClass = "bi-graph-up" }
                );
            }

            await context.SaveChangesAsync();
        }
    }
}
