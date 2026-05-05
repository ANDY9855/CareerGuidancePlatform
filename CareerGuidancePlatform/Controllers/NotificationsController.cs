using CareerGuidancePlatform.Data;
using CareerGuidancePlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CareerGuidancePlatform.Controllers
{
    [Authorize]
    public class NotificationsController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public NotificationsController(AppDbContext db, UserManager<ApplicationUser> userManager)
        { _db = db; _userManager = userManager; }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var notifications = await _db.Notifications
                .Where(n => n.UserId == user!.Id)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
            return View(notifications);
        }

        [HttpPost]
        public async Task<IActionResult> MarkRead(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var n = await _db.Notifications.FindAsync(id);
            if (n != null && n.UserId == user!.Id)
            {
                n.IsRead = true;
                await _db.SaveChangesAsync();
                if (!string.IsNullOrEmpty(n.Url))
                    return Redirect(n.Url);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> MarkAllRead()
        {
            var user = await _userManager.GetUserAsync(User);
            var unread = await _db.Notifications
                .Where(n => n.UserId == user!.Id && !n.IsRead)
                .ToListAsync();
            unread.ForEach(n => n.IsRead = true);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // AJAX endpoint for notification bell badge count
        [HttpGet]
        public async Task<IActionResult> UnreadCount()
        {
            var user = await _userManager.GetUserAsync(User);
            var count = await _db.Notifications.CountAsync(n => n.UserId == user!.Id && !n.IsRead);
            return Json(new { count });
        }

        // AJAX endpoint for dropdown preview
        [HttpGet]
        public async Task<IActionResult> Preview()
        {
            var user = await _userManager.GetUserAsync(User);
            var notifications = await _db.Notifications
                .Where(n => n.UserId == user!.Id)
                .OrderByDescending(n => n.CreatedAt)
                .Take(5)
                .ToListAsync();
            return Json(notifications.Select(n => new
            {
                n.Id, n.Title, n.Message, n.IsRead, n.Icon, n.IconColor, n.Url,
                time = n.CreatedAt.ToString("MMM dd h:mm tt")
            }));
        }
    }
}
