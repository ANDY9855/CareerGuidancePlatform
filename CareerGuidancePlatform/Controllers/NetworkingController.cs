using CareerGuidancePlatform.Data;
using CareerGuidancePlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CareerGuidancePlatform.Controllers
{
    public class NetworkingController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public NetworkingController(AppDbContext db, UserManager<ApplicationUser> userManager)
        { _db = db; _userManager = userManager; }

        public async Task<IActionResult> Events(string? industry)
        {
            var query = _db.NetworkingEvents.AsQueryable();
            if (!string.IsNullOrEmpty(industry)) query = query.Where(e => e.Industry == industry);
            ViewBag.Industries = await _db.NetworkingEvents.Select(e => e.Industry).Distinct().ToListAsync();
            return View(await query.OrderBy(e => e.EventDate).ToListAsync());
        }

        public async Task<IActionResult> Community()
        {
            var posts = await _db.PeerPosts.Include(p => p.User)
                .OrderByDescending(p => p.PostedAt).Take(20).ToListAsync();
            return View(posts);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post(string content, string category)
        {
            var user = await _userManager.GetUserAsync(User);
            _db.PeerPosts.Add(new PeerPost { UserId = user!.Id, Content = content, Category = category });
            await _db.SaveChangesAsync();
            return RedirectToAction("Community");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Like(int postId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Community");

            var post = await _db.PeerPosts.Include(p => p.PostLikes).FirstOrDefaultAsync(p => p.Id == postId);
            if (post != null) 
            { 
                var existingLike = post.PostLikes.FirstOrDefault(l => l.UserId == user.Id);
                if (existingLike == null)
                {
                    post.PostLikes.Add(new PostLike { UserId = user.Id, PostId = post.Id });
                    post.Likes++;
                }
                else
                {
                    post.PostLikes.Remove(existingLike);
                    post.Likes--;
                }
                await _db.SaveChangesAsync(); 
            }
            return RedirectToAction("Community");
        }
    }
}
