using CareerGuidancePlatform.Data;
using CareerGuidancePlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CareerGuidancePlatform.Controllers
{
    public class JobsController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public JobsController(AppDbContext db, UserManager<ApplicationUser> userManager)
        { _db = db; _userManager = userManager; }

        public async Task<IActionResult> Index(string? type, string? industry, string? search)
        {
            var query = _db.JobListings.Where(j => j.IsActive && j.Deadline >= DateTime.Now);
            if (!string.IsNullOrEmpty(type)) query = query.Where(j => j.Type == type);
            if (!string.IsNullOrEmpty(industry)) query = query.Where(j => j.Industry == industry);
            if (!string.IsNullOrEmpty(search)) query = query.Where(j => j.Title.Contains(search) || j.Company.Contains(search));
            ViewBag.Types = await _db.JobListings.Select(j => j.Type).Distinct().ToListAsync();
            ViewBag.Industries = await _db.JobListings.Select(j => j.Industry).Distinct().ToListAsync();
            ViewBag.SelectedType = type;
            ViewBag.SelectedIndustry = industry;
            ViewBag.Search = search;
            return View(await query.OrderByDescending(j => j.PostedAt).ToListAsync());
        }

        public async Task<IActionResult> Detail(int id)
        {
            var job = await _db.JobListings.Include(j => j.Reviews).FirstOrDefaultAsync(j => j.Id == id);
            if (job == null) return NotFound();
            return View(job);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Apply(int jobId)
        {
            var user = await _userManager.GetUserAsync(User);
            bool alreadyApplied = await _db.JobApplications.AnyAsync(a => a.UserId == user!.Id && a.JobListingId == jobId);
            if (!alreadyApplied)
            {
                _db.JobApplications.Add(new JobApplication { UserId = user!.Id, JobListingId = jobId });
                await _db.SaveChangesAsync();
                TempData["Success"] = "Application submitted successfully!";
            }
            else TempData["Error"] = "You have already applied for this job.";
            return RedirectToAction("Detail", new { id = jobId });
        }

        [Authorize]
        public async Task<IActionResult> Tracker()
        {
            var user = await _userManager.GetUserAsync(User);
            var apps = await _db.JobApplications
                .Include(a => a.JobListing)
                .Where(a => a.UserId == user!.Id)
                .OrderByDescending(a => a.AppliedAt)
                .ToListAsync();
            return View(apps);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int appId, string status)
        {
            var app = await _db.JobApplications.FindAsync(appId);
            if (app != null) { app.Status = status; await _db.SaveChangesAsync(); }
            return RedirectToAction("Tracker");
        }
    }
}
