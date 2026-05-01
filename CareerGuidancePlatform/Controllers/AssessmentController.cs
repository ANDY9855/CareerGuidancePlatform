using CareerGuidancePlatform.Data;
using CareerGuidancePlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CareerGuidancePlatform.Controllers
{
    [Authorize]
    public class AssessmentController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public AssessmentController(AppDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var questions = await _db.AssessmentQuestions.OrderBy(q => q.OrderIndex).ToListAsync();
            return View(questions);
        }

        [HttpPost]
        public async Task<IActionResult> Submit(Dictionary<int, string> answers)
        {
            var user = await _userManager.GetUserAsync(User);
            int techScore = 0, creativeScore = 0, analyticalScore = 0, leadershipScore = 0;

            foreach (var answer in answers)
            {
                switch (answer.Value)
                {
                    case "A": techScore++; break;
                    case "B": creativeScore++; break;
                    case "C": analyticalScore++; break;
                    case "D": leadershipScore++; break;
                }
            }

            string suggested = "Software Engineer";
            int maxScore = Math.Max(Math.Max(techScore, creativeScore), Math.Max(analyticalScore, leadershipScore));
            if (maxScore == creativeScore) suggested = "UX/UI Designer";
            else if (maxScore == analyticalScore) suggested = "Data Scientist";
            else if (maxScore == leadershipScore) suggested = "Product Manager";

            var result = new AssessmentResult
            {
                UserId = user!.Id,
                TechScore = techScore * 12,
                CreativeScore = creativeScore * 12,
                AnalyticalScore = analyticalScore * 12,
                LeadershipScore = leadershipScore * 12,
                SuggestedCareer = suggested,
                CareerCompatibilityDetails = $"Tech: {techScore * 12}% | Creative: {creativeScore * 12}% | Analytical: {analyticalScore * 12}% | Leadership: {leadershipScore * 12}%"
            };

            _db.AssessmentResults.Add(result);
            await _db.SaveChangesAsync();
            return RedirectToAction("Results", new { id = result.Id });
        }

        public async Task<IActionResult> Results(int id)
        {
            var result = await _db.AssessmentResults.FindAsync(id);
            if (result == null) return NotFound();
            return View(result);
        }

        public async Task<IActionResult> History()
        {
            var user = await _userManager.GetUserAsync(User);
            var results = await _db.AssessmentResults
                .Where(r => r.UserId == user!.Id)
                .OrderByDescending(r => r.TakenAt)
                .ToListAsync();
            return View(results);
        }
    }
}
