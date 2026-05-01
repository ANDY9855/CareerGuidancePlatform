using CareerGuidancePlatform.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CareerGuidancePlatform.Controllers
{
    public class ResourcesController : Controller
    {
        private readonly AppDbContext _db;
        public ResourcesController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Index(string? type, string? industry, string? skill, string? search)
        {
            var query = _db.Resources.AsQueryable();
            if (!string.IsNullOrEmpty(type)) query = query.Where(r => r.Type == type);
            if (!string.IsNullOrEmpty(industry)) query = query.Where(r => r.Industry == industry);
            if (!string.IsNullOrEmpty(skill)) query = query.Where(r => r.SkillCategory == skill);
            if (!string.IsNullOrEmpty(search)) query = query.Where(r => r.Title.Contains(search) || r.Description.Contains(search));

            ViewBag.Types = await _db.Resources.Select(r => r.Type).Distinct().ToListAsync();
            ViewBag.Industries = await _db.Resources.Select(r => r.Industry).Distinct().ToListAsync();
            ViewBag.Skills = await _db.Resources.Select(r => r.SkillCategory).Distinct().ToListAsync();
            ViewBag.SelectedType = type;
            ViewBag.SelectedIndustry = industry;
            ViewBag.Search = search;

            return View(await query.ToListAsync());
        }

        public async Task<IActionResult> Detail(int id)
        {
            var resource = await _db.Resources.FindAsync(id);
            if (resource == null) return NotFound();
            var related = await _db.Resources.Where(r => r.Industry == resource.Industry && r.Id != id).Take(3).ToListAsync();
            ViewBag.Related = related;
            return View(resource);
        }
    }
}
