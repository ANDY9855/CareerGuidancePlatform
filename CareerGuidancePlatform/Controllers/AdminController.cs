using CareerGuidancePlatform.Data;
using CareerGuidancePlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CareerGuidancePlatform.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(AppDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // ─────────────────────────────────────────
        // DASHBOARD
        // ─────────────────────────────────────────
        public async Task<IActionResult> Index()
        {
            ViewBag.TotalUsers        = await _userManager.Users.CountAsync();
            ViewBag.TotalJobs         = await _db.JobListings.CountAsync();
            ViewBag.ActiveJobs        = await _db.JobListings.CountAsync(j => j.IsActive);
            ViewBag.TotalMentors      = await _db.Mentors.CountAsync();
            ViewBag.TotalSessions     = await _db.MentorSessions.CountAsync();
            ViewBag.TotalResources    = await _db.Resources.CountAsync();
            ViewBag.TotalEvents       = await _db.NetworkingEvents.CountAsync();
            ViewBag.TotalApplications = await _db.JobApplications.CountAsync();
            ViewBag.TotalGoals        = await _db.Goals.CountAsync();
            ViewBag.TotalPosts        = await _db.PeerPosts.CountAsync();

            ViewBag.RecentApplications = await _db.JobApplications
                .Include(a => a.User).Include(a => a.JobListing)
                .OrderByDescending(a => a.AppliedAt).Take(6).ToListAsync();

            ViewBag.RecentUsers = await _userManager.Users
                .OrderByDescending(u => u.CreatedAt).Take(6).ToListAsync();

            ViewBag.RecentSessions = await _db.MentorSessions
                .Include(s => s.Mentor).Include(s => s.User)
                .OrderByDescending(s => s.ScheduledAt).Take(5).ToListAsync();

            return View();
        }

        // ─────────────────────────────────────────
        // JOBS
        // ─────────────────────────────────────────
        public async Task<IActionResult> ManageJobs()
        {
            var jobs = await _db.JobListings
                .Include(j => j.Applications)
                .OrderByDescending(j => j.PostedAt).ToListAsync();
            return View(jobs);
        }

        [HttpGet] public IActionResult CreateJob() => View(new JobListing { Deadline = DateTime.Now.AddDays(30) });

        [HttpPost]
        public async Task<IActionResult> CreateJob(JobListing model)
        {
            ModelState.Remove("Applications"); ModelState.Remove("Reviews");
            if (!ModelState.IsValid) return View(model);
            model.PostedAt = DateTime.UtcNow; model.IsActive = true;
            _db.JobListings.Add(model);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Job listing created successfully!";
            return RedirectToAction(nameof(ManageJobs));
        }

        [HttpGet]
        public async Task<IActionResult> EditJob(int id)
        {
            var job = await _db.JobListings.FindAsync(id);
            if (job == null) return NotFound();
            return View(job);
        }

        [HttpPost]
        public async Task<IActionResult> EditJob(JobListing model)
        {
            ModelState.Remove("Applications"); ModelState.Remove("Reviews");
            if (!ModelState.IsValid) return View(model);
            var job = await _db.JobListings.FindAsync(model.Id);
            if (job == null) return NotFound();
            job.Title = model.Title; job.Company = model.Company;
            job.Location = model.Location; job.Type = model.Type;
            job.Industry = model.Industry; job.Description = model.Description;
            job.Requirements = model.Requirements; job.SalaryRange = model.SalaryRange;
            job.Deadline = model.Deadline; job.IsActive = model.IsActive;
            await _db.SaveChangesAsync();
            TempData["Success"] = "Job updated!";
            return RedirectToAction(nameof(ManageJobs));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteJob(int id)
        {
            var job = await _db.JobListings.FindAsync(id);
            if (job != null) { _db.JobListings.Remove(job); await _db.SaveChangesAsync(); }
            TempData["Success"] = "Job deleted.";
            return RedirectToAction(nameof(ManageJobs));
        }

        [HttpPost]
        public async Task<IActionResult> ToggleJob(int id)
        {
            var job = await _db.JobListings.FindAsync(id);
            if (job != null) { job.IsActive = !job.IsActive; await _db.SaveChangesAsync(); }
            return RedirectToAction(nameof(ManageJobs));
        }

        // ─────────────────────────────────────────
        // MENTORS
        // ─────────────────────────────────────────
        public async Task<IActionResult> ManageMentors()
        {
            var mentors = await _db.Mentors
                .Include(m => m.Sessions)
                .OrderByDescending(m => m.Rating).ToListAsync();
            return View(mentors);
        }

        [HttpGet] public IActionResult CreateMentor() => View(new Mentor { Rating = 5.0, IsAvailable = true });

        [HttpPost]
        public async Task<IActionResult> CreateMentor(Mentor model)
        {
            ModelState.Remove("Sessions");
            if (!ModelState.IsValid) return View(model);
            _db.Mentors.Add(model);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Mentor added!";
            return RedirectToAction(nameof(ManageMentors));
        }

        [HttpGet]
        public async Task<IActionResult> EditMentor(int id)
        {
            var m = await _db.Mentors.FindAsync(id);
            if (m == null) return NotFound();
            return View(m);
        }

        [HttpPost]
        public async Task<IActionResult> EditMentor(Mentor model)
        {
            ModelState.Remove("Sessions");
            if (!ModelState.IsValid) return View(model);
            var m = await _db.Mentors.FindAsync(model.Id);
            if (m == null) return NotFound();
            m.Name = model.Name; m.Specialization = model.Specialization;
            m.Industry = model.Industry; m.Bio = model.Bio;
            m.Email = model.Email; m.YearsOfExperience = model.YearsOfExperience;
            m.Availability = model.Availability; m.Rating = model.Rating;
            m.IsAvailable = model.IsAvailable;
            await _db.SaveChangesAsync();
            TempData["Success"] = "Mentor updated!";
            return RedirectToAction(nameof(ManageMentors));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMentor(int id)
        {
            var m = await _db.Mentors.FindAsync(id);
            if (m != null) { _db.Mentors.Remove(m); await _db.SaveChangesAsync(); }
            TempData["Success"] = "Mentor removed.";
            return RedirectToAction(nameof(ManageMentors));
        }

        [HttpPost]
        public async Task<IActionResult> ToggleMentor(int id)
        {
            var m = await _db.Mentors.FindAsync(id);
            if (m != null) { m.IsAvailable = !m.IsAvailable; await _db.SaveChangesAsync(); }
            return RedirectToAction(nameof(ManageMentors));
        }

        // ─────────────────────────────────────────
        // USERS
        // ─────────────────────────────────────────
        public async Task<IActionResult> ManageUsers()
        {
            var users = await _userManager.Users.OrderByDescending(u => u.CreatedAt).ToListAsync();
            var userRoles = new Dictionary<string, string>();
            foreach (var u in users)
            {
                var roles = await _userManager.GetRolesAsync(u);
                userRoles[u.Id] = roles.FirstOrDefault() ?? "User";
            }
            ViewBag.UserRoles = userRoles;
            ViewBag.AllRoles = new[] { "Admin", "User", "Mentor" };
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeRole(string userId, string newRole)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var currentRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                await _userManager.AddToRoleAsync(user, newRole);
                TempData["Success"] = $"Role changed to '{newRole}' for {user.FullName}.";
            }
            return RedirectToAction(nameof(ManageUsers));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
                TempData["Success"] = "User deleted.";
            }
            return RedirectToAction(nameof(ManageUsers));
        }

        // ─────────────────────────────────────────
        // RESOURCES
        // ─────────────────────────────────────────
        public async Task<IActionResult> ManageResources()
        {
            var res = await _db.Resources.OrderByDescending(r => r.CreatedAt).ToListAsync();
            return View(res);
        }

        [HttpGet] public IActionResult CreateResource() => View(new Resource { IsFree = true, DifficultyLevel = 1 });

        [HttpPost]
        public async Task<IActionResult> CreateResource(Resource model)
        {
            if (!ModelState.IsValid) return View(model);
            model.CreatedAt = DateTime.UtcNow;
            _db.Resources.Add(model);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Resource added!";
            return RedirectToAction(nameof(ManageResources));
        }

        [HttpGet]
        public async Task<IActionResult> EditResource(int id)
        {
            var r = await _db.Resources.FindAsync(id);
            if (r == null) return NotFound();
            return View(r);
        }

        [HttpPost]
        public async Task<IActionResult> EditResource(Resource model)
        {
            if (!ModelState.IsValid) return View(model);
            var r = await _db.Resources.FindAsync(model.Id);
            if (r == null) return NotFound();
            r.Title = model.Title; r.Description = model.Description;
            r.Type = model.Type; r.Industry = model.Industry;
            r.SkillCategory = model.SkillCategory; r.Url = model.Url;
            r.Provider = model.Provider; r.IsFree = model.IsFree;
            r.Duration = model.Duration; r.DifficultyLevel = model.DifficultyLevel;
            await _db.SaveChangesAsync();
            TempData["Success"] = "Resource updated!";
            return RedirectToAction(nameof(ManageResources));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteResource(int id)
        {
            var r = await _db.Resources.FindAsync(id);
            if (r != null) { _db.Resources.Remove(r); await _db.SaveChangesAsync(); }
            TempData["Success"] = "Resource deleted.";
            return RedirectToAction(nameof(ManageResources));
        }

        // ─────────────────────────────────────────
        // EVENTS
        // ─────────────────────────────────────────
        public async Task<IActionResult> ManageEvents()
        {
            var events = await _db.NetworkingEvents.OrderBy(e => e.EventDate).ToListAsync();
            return View(events);
        }

        [HttpGet] public IActionResult CreateEvent() =>
            View(new NetworkingEvent { EventDate = DateTime.Now.AddDays(14), IconClass = "bi-calendar-event" });

        [HttpPost]
        public async Task<IActionResult> CreateEvent(NetworkingEvent model)
        {
            if (!ModelState.IsValid) return View(model);
            _db.NetworkingEvents.Add(model);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Event created!";
            return RedirectToAction(nameof(ManageEvents));
        }

        [HttpGet]
        public async Task<IActionResult> EditEvent(int id)
        {
            var e = await _db.NetworkingEvents.FindAsync(id);
            if (e == null) return NotFound();
            return View(e);
        }

        [HttpPost]
        public async Task<IActionResult> EditEvent(NetworkingEvent model)
        {
            if (!ModelState.IsValid) return View(model);
            var e = await _db.NetworkingEvents.FindAsync(model.Id);
            if (e == null) return NotFound();
            e.Title = model.Title; e.Description = model.Description;
            e.EventType = model.EventType; e.Organizer = model.Organizer;
            e.Location = model.Location; e.EventDate = model.EventDate;
            e.RegistrationUrl = model.RegistrationUrl;
            e.IsOnline = model.IsOnline; e.Industry = model.Industry;
            e.IconClass = model.IconClass;
            await _db.SaveChangesAsync();
            TempData["Success"] = "Event updated!";
            return RedirectToAction(nameof(ManageEvents));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var e = await _db.NetworkingEvents.FindAsync(id);
            if (e != null) { _db.NetworkingEvents.Remove(e); await _db.SaveChangesAsync(); }
            TempData["Success"] = "Event deleted.";
            return RedirectToAction(nameof(ManageEvents));
        }

        // ─────────────────────────────────────────
        // SESSIONS
        // ─────────────────────────────────────────
        public async Task<IActionResult> ManageSessions()
        {
            var sessions = await _db.MentorSessions
                .Include(s => s.Mentor).Include(s => s.User)
                .OrderByDescending(s => s.ScheduledAt).ToListAsync();
            return View(sessions);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSessionStatus(int sessionId, string status)
        {
            var s = await _db.MentorSessions.FindAsync(sessionId);
            if (s != null) { s.Status = status; await _db.SaveChangesAsync(); TempData["Success"] = "Status updated."; }
            return RedirectToAction(nameof(ManageSessions));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSession(int id)
        {
            var s = await _db.MentorSessions.FindAsync(id);
            if (s != null) { _db.MentorSessions.Remove(s); await _db.SaveChangesAsync(); }
            TempData["Success"] = "Session removed.";
            return RedirectToAction(nameof(ManageSessions));
        }

        // ─────────────────────────────────────────
        // APPLICATIONS
        // ─────────────────────────────────────────
        public async Task<IActionResult> ManageApplications()
        {
            var apps = await _db.JobApplications
                .Include(a => a.User).Include(a => a.JobListing)
                .OrderByDescending(a => a.AppliedAt).ToListAsync();
            return View(apps);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteApplication(int id)
        {
            var a = await _db.JobApplications.FindAsync(id);
            if (a != null) { _db.JobApplications.Remove(a); await _db.SaveChangesAsync(); }
            TempData["Success"] = "Application removed.";
            return RedirectToAction(nameof(ManageApplications));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateApplicationStatus(int id, string status)
        {
            var a = await _db.JobApplications.FindAsync(id);
            if (a != null) { a.Status = status; await _db.SaveChangesAsync(); TempData["Success"] = "Status updated."; }
            return RedirectToAction(nameof(ManageApplications));
        }

        // ─────────────────────────────────────────
        // COMMUNITY POSTS
        // ─────────────────────────────────────────
        public async Task<IActionResult> ManagePosts()
        {
            var posts = await _db.PeerPosts
                .Include(p => p.User)
                .OrderByDescending(p => p.PostedAt).ToListAsync();
            return View(posts);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = await _db.PeerPosts.FindAsync(id);
            if (post != null) { _db.PeerPosts.Remove(post); await _db.SaveChangesAsync(); }
            TempData["Success"] = "Post removed.";
            return RedirectToAction(nameof(ManagePosts));
        }
    }
}
