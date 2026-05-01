using CareerGuidancePlatform.Data;
using CareerGuidancePlatform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CareerGuidancePlatform.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;
        public HomeController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            ViewBag.MentorCount = await _db.Mentors.CountAsync();
            ViewBag.JobCount = await _db.JobListings.CountAsync(j => j.IsActive);
            ViewBag.ResourceCount = await _db.Resources.CountAsync();
            ViewBag.CareerPathCount = await _db.CareerPaths.CountAsync();
            ViewBag.FeaturedJobs = await _db.JobListings.Where(j => j.IsActive).Take(3).ToListAsync();
            ViewBag.FeaturedMentors = await _db.Mentors.Where(m => m.IsAvailable).OrderByDescending(m => m.Rating).Take(3).ToListAsync();
            return View();
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
