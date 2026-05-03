using CareerGuidancePlatform.Data;
using CareerGuidancePlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;

namespace CareerGuidancePlatform.Controllers
{
    [Authorize(Roles = "Mentor")]
    public class MentorDashboardController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public MentorDashboardController(AppDbContext db, UserManager<ApplicationUser> userManager)
        { _db = db; _userManager = userManager; }

        // Find the Mentor record linked to the logged-in user's email
        private async Task<Mentor?> GetMyMentor()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return null;
            return await _db.Mentors.FirstOrDefaultAsync(m => m.Email == user.Email);
        }

        // ─────── DASHBOARD ───────
        public async Task<IActionResult> Index()
        {
            var mentor = await GetMyMentor();
            if (mentor == null) return View("NoProfile");

            ViewBag.TotalSessions  = await _db.MentorSessions.CountAsync(s => s.MentorId == mentor.Id);
            ViewBag.Pending        = await _db.MentorSessions.CountAsync(s => s.MentorId == mentor.Id && s.Status == "Pending");
            ViewBag.Confirmed      = await _db.MentorSessions.CountAsync(s => s.MentorId == mentor.Id && s.Status == "Confirmed");
            ViewBag.Completed      = await _db.MentorSessions.CountAsync(s => s.MentorId == mentor.Id && s.Status == "Completed");
            ViewBag.UnreadMessages = await _db.MentorMessages.CountAsync(m => m.MentorId == mentor.Id && !m.IsRead && !m.IsFromMentor);

            ViewBag.UpcomingSessions = await _db.MentorSessions
                .Include(s => s.User)
                .Where(s => s.MentorId == mentor.Id && s.Status != "Cancelled" && s.ScheduledAt >= DateTime.Now)
                .OrderBy(s => s.ScheduledAt)
                .Take(5)
                .ToListAsync();

            ViewBag.RecentMessages = await _db.MentorMessages
                .Include(m => m.User)
                .Where(m => m.MentorId == mentor.Id && !m.IsFromMentor)
                .OrderByDescending(m => m.SentAt)
                .Take(4)
                .ToListAsync();

            return View(mentor);
        }

        // ─────── SESSIONS ───────
        public async Task<IActionResult> Sessions()
        {
            var mentor = await GetMyMentor();
            if (mentor == null) return View("NoProfile");

            var sessions = await _db.MentorSessions
                .Include(s => s.User)
                .Where(s => s.MentorId == mentor.Id)
                .OrderByDescending(s => s.ScheduledAt)
                .ToListAsync();

            ViewBag.MentorName = mentor.Name;
            return View(sessions);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSession(int sessionId, string status)
        {
            var mentor = await GetMyMentor();
            if (mentor == null) return Unauthorized();

            var session = await _db.MentorSessions.FindAsync(sessionId);
            if (session != null && session.MentorId == mentor.Id)
            {
                session.Status = status;
                if (status == "Completed")
                {
                    mentor.TotalSessions++;
                }
                await _db.SaveChangesAsync();
                TempData["Success"] = $"Session marked as {status}.";
            }
            return RedirectToAction(nameof(Sessions));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSession(int sessionId)
        {
            var mentor = await GetMyMentor();
            if (mentor == null) return Unauthorized();

            var session = await _db.MentorSessions.FindAsync(sessionId);
            if (session != null && session.MentorId == mentor.Id)
            {
                _db.MentorSessions.Remove(session);
                await _db.SaveChangesAsync();
                TempData["Success"] = "Session removed.";
            }
            return RedirectToAction(nameof(Sessions));
        }

        // ─────── MESSAGES ───────
        public async Task<IActionResult> Messages(string? userId)
        {
            var mentor = await GetMyMentor();
            if (mentor == null) return View("NoProfile");

            // Get all unique user IDs who have conversations with this mentor
            var allUserIds = await _db.MentorMessages
                .Where(m => m.MentorId == mentor.Id)
                .Select(m => m.UserId)
                .Distinct()
                .ToListAsync();

            var vm = new MentorMessagesViewModel { Mentor = mentor, SelectedUserId = userId };

            foreach (var uid in allUserIds)
            {
                var u = await _userManager.FindByIdAsync(uid);
                if (u == null) continue;
                var last = await _db.MentorMessages
                    .Where(m => m.MentorId == mentor.Id && m.UserId == uid)
                    .OrderByDescending(m => m.SentAt).FirstOrDefaultAsync();
                var unread = await _db.MentorMessages
                    .CountAsync(m => m.MentorId == mentor.Id && m.UserId == uid && !m.IsRead && !m.IsFromMentor);
                if (last != null)
                    vm.Conversations.Add(new MentorConversationItem { User = u, LastMessage = last, UnreadCount = unread });
            }

            if (!string.IsNullOrEmpty(userId))
            {
                vm.SelectedUser = await _userManager.FindByIdAsync(userId);
                vm.Thread = await _db.MentorMessages
                    .Where(m => m.MentorId == mentor.Id && m.UserId == userId)
                    .OrderBy(m => m.SentAt).ToListAsync();

                // Mark incoming messages as read
                var unreadMsgs = vm.Thread.Where(m => !m.IsRead && !m.IsFromMentor).ToList();
                unreadMsgs.ForEach(m => m.IsRead = true);
                if (unreadMsgs.Any()) await _db.SaveChangesAsync();
            }

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> ReplyMessage(int mentorId, string userId, string content, [FromServices] Microsoft.AspNetCore.SignalR.IHubContext<CareerGuidancePlatform.Hubs.ChatHub> hubContext)
        {
            var mentor = await GetMyMentor();
            if (mentor == null || mentor.Id != mentorId) return Unauthorized();

            if (!string.IsNullOrWhiteSpace(content))
            {
                var msg = new MentorMessage
                {
                    MentorId = mentorId,
                    UserId = userId,
                    Content = content.Trim(),
                    IsFromMentor = true,
                    SentAt = DateTime.UtcNow,
                    IsRead = false
                };
                _db.MentorMessages.Add(msg);
                await _db.SaveChangesAsync();

                // Send real-time update to the specific chat group
                await hubContext.Clients.Group($"Chat_{mentorId}_{userId}").SendAsync("ReceiveMessage", new
                {
                    content = msg.Content,
                    isFromMentor = true,
                    sentAt = msg.SentAt.ToString("MMM dd, h:mm tt")
                });

                return Json(new { success = true });
            }
            return BadRequest();
        }

        // ─────── PROFILE ───────
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var mentor = await GetMyMentor();
            if (mentor == null) return View("NoProfile");
            return View(mentor);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(Mentor model)
        {
            var mentor = await GetMyMentor();
            if (mentor == null) return Unauthorized();

            mentor.Bio = model.Bio;
            mentor.Specialization = model.Specialization;
            mentor.Industry = model.Industry;
            mentor.Availability = model.Availability;
            mentor.IsAvailable = model.IsAvailable;
            await _db.SaveChangesAsync();
            TempData["Success"] = "Profile updated successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
