using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WorkshopManager.DTOs;
using WorkshopManager.Services;

namespace WorkshopManager.Pages.Parts
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly IPartCatalogItemService _catalogService;

        public CreateModel(IPartCatalogItemService catalogService)
        {
            _catalogService = catalogService;
        }

        [BindProperty]
        public PartCatalogItemCreateDto NewPart { get; set; } = new();

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            await _catalogService.CreateAsync(NewPart);
            return Page();
        }
    }
}
