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
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalUsers = await _userManager.Users.CountAsync();
            ViewBag.TotalJobs = await _context.JobListings.CountAsync();
            ViewBag.TotalMentors = await _context.Mentors.CountAsync();
            ViewBag.TotalSessions = await _context.MentorSessions.CountAsync();
            
            var recentActivity = await _context.PeerPosts
                .Include(p => p.User)
                .OrderByDescending(p => p.PostedAt)
                .Take(5)
                .ToListAsync();

            return View(recentActivity);
        }

        // --- MANAGE JOBS ---
        public async Task<IActionResult> ManageJobs()
        {
            var jobs = await _context.JobListings.OrderByDescending(j => j.PostedAt).ToListAsync();
            return View(jobs);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteJob(int id)
        {
            var job = await _context.JobListings.FindAsync(id);
            if (job != null)
            {
                _context.JobListings.Remove(job);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(ManageJobs));
        }

        // --- MANAGE MENTORS ---
        public async Task<IActionResult> ManageMentors()
        {
            var mentors = await _context.Mentors.OrderByDescending(m => m.Rating).ToListAsync();
            return View(mentors);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMentor(int id)
        {
            var mentor = await _context.Mentors.FindAsync(id);
            if (mentor != null)
            {
                _context.Mentors.Remove(mentor);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(ManageMentors));
        }
    }
}
