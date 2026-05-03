using CareerGuidancePlatform.Data;
using CareerGuidancePlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CareerGuidancePlatform.Controllers
{
    [Authorize]
    public class ResumeController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public ResumeController(AppDbContext db, UserManager<ApplicationUser> userManager, IWebHostEnvironment env)
        { 
            _db = db; 
            _userManager = userManager; 
            _env = env;
        }

        public async Task<IActionResult> Builder()
        {
            var user = await _userManager.GetUserAsync(User);
            var resume = await _db.ResumeData
                .Include(r => r.Experiences)
                .Include(r => r.Educations)
                .Include(r => r.Certifications)
                .FirstOrDefaultAsync(r => r.UserId == user!.Id);
            if (resume == null)
                resume = new ResumeData { UserId = user!.Id, FullName = user.FullName, Email = user.Email ?? "" };
            return View(resume);
        }

        [HttpPost]
        public async Task<IActionResult> Save(ResumeData model, IFormFile? profilePicture,
            List<string> jobTitles, List<string> companies, List<string> startDates, List<string> endDates, List<string> expDescriptions,
            List<string> degrees, List<string> institutions, List<string> years,
            List<string> certNames, List<string> certIssuers, List<string> certYears)
        {
            var user = await _userManager.GetUserAsync(User);
            var existing = await _db.ResumeData
                .Include(r => r.Experiences)
                .Include(r => r.Educations)
                .Include(r => r.Certifications)
                .FirstOrDefaultAsync(r => r.UserId == user!.Id);

            model.FullName ??= string.Empty;
            model.Email ??= string.Empty;
            model.Phone ??= string.Empty;
            model.Location ??= string.Empty;
            model.LinkedIn ??= string.Empty;
            model.Portfolio ??= string.Empty;
            model.Summary ??= string.Empty;
            model.Skills ??= string.Empty;
            model.Template ??= "Classic";

            string? pfpPath = existing?.ProfilePicturePath;
            if (profilePicture != null && profilePicture.Length > 0)
            {
                var folder = Path.Combine(_env.WebRootPath, "uploads", "resumes");
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(profilePicture.FileName);
                var filePath = Path.Combine(folder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await profilePicture.CopyToAsync(stream);
                }
                pfpPath = "/uploads/resumes/" + fileName;
            }

            if (existing == null)
            {
                model.UserId = user!.Id;
                model.ProfilePicturePath = pfpPath;
                model.LastUpdated = DateTime.UtcNow;
                _db.ResumeData.Add(model);
            }
            else
            {
                existing.FullName = model.FullName;
                existing.Email = model.Email;
                existing.Phone = model.Phone;
                existing.Location = model.Location;
                existing.LinkedIn = model.LinkedIn;
                existing.Portfolio = model.Portfolio;
                existing.Summary = model.Summary;
                existing.Skills = model.Skills;
                existing.Template = model.Template;
                existing.ProfilePicturePath = pfpPath;
                existing.LastUpdated = DateTime.UtcNow;
                existing.Experiences.Clear();
                existing.Educations.Clear();
                existing.Certifications.Clear();
                model = existing;
            }

            for (int i = 0; i < jobTitles.Count; i++)
                if (!string.IsNullOrWhiteSpace(jobTitles[i]))
                    model.Experiences.Add(new ResumeExperience { JobTitle = jobTitles[i], Company = companies[i], StartDate = startDates[i], EndDate = endDates[i], Description = expDescriptions[i] });

            for (int i = 0; i < degrees.Count; i++)
                if (!string.IsNullOrWhiteSpace(degrees[i]))
                    model.Educations.Add(new ResumeEducation { Degree = degrees[i], Institution = institutions[i], Year = years[i] });

            for (int i = 0; i < certNames.Count; i++)
                if (!string.IsNullOrWhiteSpace(certNames[i]))
                    model.Certifications.Add(new ResumeCertification { Name = certNames[i], IssuedBy = certIssuers[i], Year = certYears[i] });

            await _db.SaveChangesAsync();
            return RedirectToAction("Preview");
        }

        public async Task<IActionResult> Preview()
        {
            var user = await _userManager.GetUserAsync(User);
            var resume = await _db.ResumeData
                .Include(r => r.Experiences)
                .Include(r => r.Educations)
                .Include(r => r.Certifications)
                .FirstOrDefaultAsync(r => r.UserId == user!.Id);
            if (resume == null) return RedirectToAction("Builder");
            return View(resume);
        }
    }
}
