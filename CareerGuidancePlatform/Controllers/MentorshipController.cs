using CareerGuidancePlatform.Data;
using CareerGuidancePlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;

namespace CareerGuidancePlatform.Controllers
{
    public class MentorshipController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public MentorshipController(AppDbContext db, UserManager<ApplicationUser> userManager)
        { _db = db; _userManager = userManager; }

        public async Task<IActionResult> Index(string? industry, string? specialization)
        {
            var query = _db.Mentors.AsQueryable();
            if (!string.IsNullOrEmpty(industry)) query = query.Where(m => m.Industry == industry);
            if (!string.IsNullOrEmpty(specialization)) query = query.Where(m => m.Specialization.Contains(specialization));
            ViewBag.Industries = await _db.Mentors.Select(m => m.Industry).Distinct().ToListAsync();
            return View(await query.Where(m => m.IsAvailable).ToListAsync());
        }

        public async Task<IActionResult> Profile(int id)
        {
            var mentor = await _db.Mentors.Include(m => m.Sessions).FirstOrDefaultAsync(m => m.Id == id);
            if (mentor == null) return NotFound();
            return View(mentor);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Book(int mentorId)
        {
            var mentor = await _db.Mentors.FindAsync(mentorId);
            if (mentor == null) return NotFound();
            ViewBag.Mentor = mentor;
            return View(new BookSessionViewModel { MentorId = mentorId });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Book(BookSessionViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            var session = new MentorSession
            {
                MentorId = model.MentorId,
                UserId = user!.Id,
                ScheduledAt = model.ScheduledAt,
                SessionType = model.SessionType,
                Topic = model.Topic,
                Status = "Pending"
            };
            _db.MentorSessions.Add(session);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Session booked! Your mentor will confirm shortly.";
            return RedirectToAction("MySessions");
        }

        [Authorize]
        public async Task<IActionResult> MySessions()
        {
            var user = await _userManager.GetUserAsync(User);
            var sessions = await _db.MentorSessions
                .Include(s => s.Mentor)
                .Where(s => s.UserId == user!.Id)
                .OrderByDescending(s => s.ScheduledAt)
                .ToListAsync();
            return View(sessions);
        }

        [Authorize]
        public async Task<IActionResult> Chat(int mentorId)
        {
            var user = await _userManager.GetUserAsync(User);
            var mentor = await _db.Mentors.FindAsync(mentorId);
            if (mentor == null) return NotFound();

            var messages = await _db.MentorMessages
                .Where(m => m.MentorId == mentorId && m.UserId == user!.Id)
                .OrderBy(m => m.SentAt)
                .ToListAsync();

            // Mark mentor replies as read
            var unread = messages.Where(m => m.IsFromMentor && !m.IsRead).ToList();
            unread.ForEach(m => m.IsRead = true);
            if (unread.Any()) await _db.SaveChangesAsync();

            ViewBag.Mentor = mentor;
            ViewBag.MentorId = mentorId;
            return View(messages);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SendMessage(int mentorId, string content, [FromServices] Microsoft.AspNetCore.SignalR.IHubContext<CareerGuidancePlatform.Hubs.ChatHub> hubContext)
        {
            var user = await _userManager.GetUserAsync(User);
            if (!string.IsNullOrWhiteSpace(content))
            {
                var msg = new MentorMessage
                {
                    MentorId = mentorId,
                    UserId = user!.Id,
                    Content = content.Trim(),
                    IsFromMentor = false,
                    SentAt = DateTime.UtcNow
                };
                _db.MentorMessages.Add(msg);
                await _db.SaveChangesAsync();

                // Send real-time update to the specific chat group
                await hubContext.Clients.Group($"Chat_{mentorId}_{user.Id}").SendAsync("ReceiveMessage", new
                {
                    content = msg.Content,
                    isFromMentor = false,
                    sentAt = msg.SentAt.ToString("MMM dd h:mm tt")
                });

                return Json(new { success = true });
            }
            return BadRequest();
        }
    }
}
