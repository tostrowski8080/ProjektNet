using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WorkshopManager.DTOs;
using WorkshopManager.Services;

namespace WorkshopManager.Pages.Parts
{
    [Authorize(Roles = "Admin,Receptionist")]
    public class UpdateModel : PageModel
    {
        private readonly IPartCatalogItemService _catalogService;

        public UpdateModel(IPartCatalogItemService catalogService)
        {
            _catalogService = catalogService;
        }

        public List<PartCatalogItemDto> CatalogItems { get; set; } = new();

        [BindProperty]
        public Dictionary<int, int> UpdatedStocks { get; set; } = new();

        public async Task OnGetAsync()
        {
            CatalogItems = (await _catalogService.GetAllAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            foreach (var kvp in UpdatedStocks)
            {
                var item = await _catalogService.GetByIdAsync(kvp.Key);
                if (item != null)
                {
                    item.Stock = kvp.Value;
                    await _catalogService.UpdateAsync(new PartCatalogItemUpdateDto
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Price = item.Price,
                        Stock = item.Stock
                    });
                }
            }

            return RedirectToPage();
        }
    }
}