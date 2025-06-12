using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WorkshopManager.Data;
using WorkshopManager.Models;

namespace WorkshopManager.Pages.Parts
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly WorkshopDbContext _context;

        public CreateModel(WorkshopDbContext context)
        {
            _context = context;
        }

        public class PartCatalogViewModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Stock { get; set; }
            public int Required { get; set; }
        }

        public List<PartCatalogViewModel> CatalogItems { get; set; } = new();

        [BindProperty]
        public PartCatalogItem NewPart { get; set; } = new();

        [BindProperty]
        public Dictionary<int, int> UpdatedStocks { get; set; } = new();

        public bool IsAdmin => User.IsInRole("Admin");

        public async Task OnGetAsync()
        {
            CatalogItems = await _context.PartCatalog
                .Select(p => new PartCatalogViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Stock = p.Stock,
                    Required = _context.Parts.Where(x => x.PartCatalogId == p.Id).Sum(x => (int?)x.Quantity) ?? 0
                }).ToListAsync();
        }

        public async Task<IActionResult> OnPostUpdateStockAsync()
        {
            foreach (var entry in UpdatedStocks)
            {
                var part = await _context.PartCatalog.FindAsync(entry.Key);
                if (part != null)
                    part.Stock = entry.Value;
            }

            await _context.SaveChangesAsync();
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostAddPartAsync()
        {
            _context.PartCatalog.Add(NewPart);
            await _context.SaveChangesAsync();
            return RedirectToPage();
        }
    }
}
