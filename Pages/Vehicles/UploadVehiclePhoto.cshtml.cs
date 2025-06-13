using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WorkshopManager.Models;
using WorkshopManager.Services;

namespace WorkshopManager.Pages.Vehicles
{
    public class UploadVehiclePhotoModel : PageModel
    {
        private readonly IVehicleService _vehicleService;
        private readonly IWebHostEnvironment _env;

        public UploadVehiclePhotoModel(
            IVehicleService vehicleService,
            IWebHostEnvironment env)
        {
            _vehicleService = vehicleService;
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
            Vehicles = (await _vehicleService.GetAllAsync())
                .Where(v => v.Client != null
                         && !string.IsNullOrEmpty(v.Client.AccountId))
                .ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await OnGetAsync();

            SelectedVehicle = Vehicles
                .FirstOrDefault(v => v.Id == SelectedVehicleId);

            if (SelectedVehicle == null)
            {
                ModelState.AddModelError(string.Empty, "Vehicle not found.");
                return Page();
            }

            if (UploadedImage == null
             || !(UploadedImage.ContentType == "image/jpeg"
               || UploadedImage.ContentType == "image/png"))
            {
                ModelState.AddModelError(string.Empty, "Only JPG/PNG allowed.");
                return Page();
            }

            var uploads = Path.Combine(_env.WebRootPath!, "uploads");
            if (!Directory.Exists(uploads))
                Directory.CreateDirectory(uploads);

            var fileName = $"vehicle_{SelectedVehicle.Id}_{Path.GetFileName(UploadedImage.FileName)}";
            var path = Path.Combine(uploads, fileName);
            using var fs = new FileStream(path, FileMode.Create);
            await UploadedImage.CopyToAsync(fs);

            SelectedVehicle.PhotoPath = $"/uploads/{fileName}";
            await _vehicleService.UpdateAsync(SelectedVehicle);

            return RedirectToPage();
        }
    }
}