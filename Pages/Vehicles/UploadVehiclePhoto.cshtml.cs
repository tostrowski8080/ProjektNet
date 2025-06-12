using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WorkshopManager.Data;
using WorkshopManager.Models;

namespace WorkshopManager.Pages.Vehicles
{
    [Authorize(Roles = "Admin,Mechanic,Receptionist,Client")]
    public class UploadVehiclePhotoModel : PageModel
    {
        private readonly IWebHostEnvironment _env;
        private readonly WorkshopDbContext _context;

        public UploadVehiclePhotoModel(WorkshopDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public List<Vehicle> Vehicles { get; set; } = new();

        [BindProperty]
        public int SelectedVehicleId { get; set; }

        [BindProperty]
        public IFormFile? UploadedImage { get; set; }

        public Vehicle? SelectedVehicle { get; set; }

        public async Task OnGetAsync()
        {
            Vehicles = await _context.Vehicles
                .Include(v => v.Client)
                .Where(v => v.Client != null && v.Client.AccountId != null && v.Client.AccountId != "")
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Vehicles = await _context.Vehicles
                .Include(v => v.Client)
                .Where(v => v.Client != null && v.Client.AccountId != null && v.Client.AccountId != "")
                .ToListAsync();

            SelectedVehicle = Vehicles.FirstOrDefault(v => v.Id == SelectedVehicleId);

            if (SelectedVehicle == null)
            {
                ModelState.AddModelError("", "Vehicle not found.");
                return Page();
            }

            if (UploadedImage == null || (UploadedImage.ContentType != "image/jpeg" && UploadedImage.ContentType != "image/png"))
            {
                ModelState.AddModelError("", "Only PNG and JPG files are allowed.");
                return Page();
            }

            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = $"vehicle_{SelectedVehicle.Id}_{Path.GetFileName(UploadedImage.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await UploadedImage.CopyToAsync(stream);
            }

            SelectedVehicle.PhotoPath = $"/uploads/{fileName}";

            _context.Vehicles.Update(SelectedVehicle);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }
    }
}
