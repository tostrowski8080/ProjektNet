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
        private readonly IPartService _partService;

        public CreateModel(
            IPartCatalogItemService catalogService,
            IPartService partService)
        {
            _catalogService = catalogService;
            _partService = partService;
        }

        public class CatalogItemViewModel
        {
            public int Id { get; set; }
            public string Name { get; set; } = "";
            public int Stock { get; set; }
            public int Required { get; set; }
        }

        public List<CatalogItemViewModel> CatalogItems { get; set; } = new();

        [BindProperty]
        public PartCatalogItemCreateDto NewPart { get; set; } = new();

        [BindProperty]
        public Dictionary<int, int> UpdatedStocks { get; set; } = new();

        public bool IsAdmin => User.IsInRole("Admin");

        public async Task OnGetAsync()
        {
            // Pobierz ca³y katalog
            var items = await _catalogService.GetAllAsync();

            // Dla ka¿dego policz sumê u¿ycia
            CatalogItems = new List<CatalogItemViewModel>();
            foreach (var dto in items)
            {
                var required = (await _partService
                    .GetByOrderIdAsync(0))   // tu mo¿esz dodaæ przeci¹¿enie GetByCatalogId
                    .Where(p => p.PartCatalogId == dto.Id)
                    .Sum(p => p.Quantity);

                CatalogItems.Add(new CatalogItemViewModel
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    Stock = dto.Stock,
                    Required = required
                });

                // przygotuj domyœlne wartoœci formularza
                UpdatedStocks[dto.Id] = dto.Stock;
            }
        }

        public async Task<IActionResult> OnPostUpdateStockAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            foreach (var kv in UpdatedStocks)
            {
                var updateDto = new PartCatalogItemUpdateDto
                {
                    Id = kv.Key,
                    Stock = kv.Value,
                    Name = CatalogItems.First(c => c.Id == kv.Key).Name,
                    Price = (await _catalogService.GetByIdAsync(kv.Key))!.Price
                };
                await _catalogService.UpdateAsync(updateDto);
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostAddPartAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            await _catalogService.CreateAsync(NewPart);
            return RedirectToPage();
        }
    }
}
