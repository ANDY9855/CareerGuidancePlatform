using CareerGuidancePlatform.Data;
using CareerGuidancePlatform.Models;
using CareerGuidancePlatform.Services;
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
        private readonly IEmailSender _emailSender;
        public MentorshipController(AppDbContext db, UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        { _db = db; _userManager = userManager; _emailSender = emailSender; }

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

            // Send email notification to mentor
            try
            {
                var mentor = await _db.Mentors.FindAsync(model.MentorId);
                if (mentor != null && !string.IsNullOrEmpty(mentor.Email))
                {
                    await _emailSender.SendEmailAsync(
                        mentor.Email, mentor.Name,
                        "New Session Booking - Career Guidance Platform",
                        EmailTemplates.SessionBooked(
                            mentor.Name,
                            user!.UserName ?? "A user",
                            model.Topic,
                            model.SessionType,
                            model.ScheduledAt.ToString("MMMM dd, yyyy h:mm tt")
                        )
                    );
                }
            }
            catch { /* Email is non-critical */ }

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

            var hasActiveSession = await _db.MentorSessions.AnyAsync(s => s.UserId == user!.Id && s.MentorId == mentorId && s.Status != "Cancelled");

            ViewBag.Mentor = mentor;
            ViewBag.MentorId = mentorId;
            ViewBag.HasActiveSession = hasActiveSession;
            return View(messages);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SendMessage(int mentorId, string content, [FromServices] Microsoft.AspNetCore.SignalR.IHubContext<CareerGuidancePlatform.Hubs.ChatHub> hubContext)
        {
            var user = await _userManager.GetUserAsync(User);
            
            var hasActiveSession = await _db.MentorSessions.AnyAsync(s => s.UserId == user!.Id && s.MentorId == mentorId && s.Status != "Cancelled");
            if (!hasActiveSession)
            {
                return BadRequest("Cannot send messages. Your session with this mentor has been cancelled.");
            }

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

        [Authorize]
        public async Task<IActionResult> VideoCall(int mentorId, bool isCaller = true, string? callerConnId = null)
        {
            var user = await _userManager.GetUserAsync(User);
            var mentor = await _db.Mentors.FindAsync(mentorId);
            if (mentor == null) return NotFound();

            var hasActiveSession = await _db.MentorSessions.AnyAsync(
                s => s.UserId == user!.Id && s.MentorId == mentorId && s.Status != "Cancelled");
            if (!hasActiveSession)
            {
                TempData["Error"] = "You need an active session to start a video call.";
                return RedirectToAction("Chat", new { mentorId });
            }

            ViewBag.Mentor = mentor;
            ViewBag.MentorId = mentorId;
            ViewBag.UserName = user!.UserName ?? user.Email;
            ViewBag.UserId = user.Id;
            ViewBag.IsCaller = isCaller;
            ViewBag.CallerConnId = callerConnId;
            return View();
        }
    }
}
