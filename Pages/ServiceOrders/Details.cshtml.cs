using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WorkshopManager.Data;
using WorkshopManager.Models;

namespace WorkshopManager.Pages.ServiceOrders
{
    public class DetailsModel : PageModel
    {
        private readonly WorkshopDbContext _context;

        public DetailsModel(WorkshopDbContext context)
        {
            _context = context;
        }

        public ServiceOrder? ServiceOrder { get; set; }
        public Vehicle? Vehicle { get; set; }
        public ApplicationUser? Mechanic { get; set; }
        public List<PartCatalogItem>? PartCatalog { get; set; } = new();

        public int TotalCost { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            ServiceOrder = await _context.ServiceOrders.Include(o => o.Tasks!.AsEnumerable()).ThenInclude(t => t.Parts)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (ServiceOrder == null)
                return NotFound();

            Vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.Id == ServiceOrder.VehicleId);
            Mechanic = ServiceOrder.WorkerId != null
                ? await _context.Users.FirstOrDefaultAsync(u => u.Id == ServiceOrder.WorkerId)
                : null;

            PartCatalog = await _context.PartCatalog.ToListAsync();

            TotalCost = ServiceOrder.Tasks?
                .Sum(t => (t.Cost ?? 0) + t.Parts?.Sum(p => p.Cost) ?? 0) ?? 0;

            return Page();
        }
    }
}
