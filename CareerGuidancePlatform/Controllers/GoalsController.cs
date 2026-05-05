using CareerGuidancePlatform.Data;
using CareerGuidancePlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CareerGuidancePlatform.Controllers
{
    [Authorize]
    public class GoalsController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public GoalsController(AppDbContext db, UserManager<ApplicationUser> userManager)
        { _db = db; _userManager = userManager; }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var goals = await _db.Goals.Include(g => g.Milestones)
                .Where(g => g.UserId == user!.Id)
                .OrderByDescending(g => g.CreatedAt).ToListAsync();
            return View(goals);
        }

        [HttpGet]
        public IActionResult Create() => View(new CreateGoalViewModel());

        [HttpPost]
        public async Task<IActionResult> Create(CreateGoalViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            var goal = new Goal
            {
                UserId = user!.Id,
                Title = model.Title,
                Description = model.Description,
                Category = model.Category,
                TermType = model.TermType,
                TargetDate = model.TargetDate
            };
            foreach (var (m, i) in model.Milestones.Where(m => !string.IsNullOrWhiteSpace(m)).Select((m, i) => (m, i)))
                goal.Milestones.Add(new GoalMilestone { Title = m, OrderIndex = i });
            _db.Goals.Add(goal);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ToggleMilestone(int milestoneId)
        {
            var goal = await _db.Goals.Include(g => g.Milestones)
                                      .FirstOrDefaultAsync(g => g.Milestones.Any(m => m.Id == milestoneId));
            if (goal != null)
            {
                var milestone = goal.Milestones.FirstOrDefault(m => m.Id == milestoneId);
                if (milestone != null)
                {
                    milestone.IsCompleted = !milestone.IsCompleted;
                    
                    var total = goal.Milestones.Count;
                    var done = goal.Milestones.Count(m => m.IsCompleted);
                    goal.ProgressPercent = total > 0 ? (int)Math.Round((double)done * 100 / total) : 0;
                    goal.IsCompleted = goal.ProgressPercent == 100;
                    
                    await _db.SaveChangesAsync();
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var goal = await _db.Goals.FirstOrDefaultAsync(g => g.Id == id && g.UserId == user!.Id);
            if (goal != null) { _db.Goals.Remove(goal); await _db.SaveChangesAsync(); }
            return RedirectToAction("Index");
        }
    }
}
