using CareerGuidancePlatform.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CareerGuidancePlatform.Controllers
{
    public class CareerPathController : Controller
    {
        private readonly AppDbContext _db;
        public CareerPathController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var paths = await _db.CareerPaths.ToListAsync();
            var stories = await _db.SuccessStories.Take(3).ToListAsync();
            ViewBag.Stories = stories;
            return View(paths);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var path = await _db.CareerPaths
                .Include(p => p.Stages.OrderBy(s => s.StageOrder))
                .FirstOrDefaultAsync(p => p.Id == id);
            if (path == null) return NotFound();
            return View(path);
        }

        public async Task<IActionResult> Stories()
        {
            var stories = await _db.SuccessStories.OrderByDescending(s => s.PublishedAt).ToListAsync();
            return View(stories);
        }

        public async Task<IActionResult> Roles()
        {
            var roles = await _db.JobRoleOverviews.ToListAsync();
            return View(roles);
        }
    }
}
