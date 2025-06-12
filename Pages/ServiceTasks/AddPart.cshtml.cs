using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using WorkshopManager.Data;
using WorkshopManager.Models;

namespace WorkshopManager.Pages.ServiceTasks
{
    [Authorize(Roles = "Admin,Mechanic")]
    public class AddPartModel : PageModel
    {
        private readonly WorkshopDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AddPartModel(WorkshopDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public int SelectedTaskId { get; set; }

        public List<ServiceTask> AvailableTasks { get; set; } = new();
        public List<PartCatalogItem> PartCatalog { get; set; } = new();

        [BindProperty]
        public List<PartInputModel> PartInputs { get; set; } = new();

        public class PartInputModel
        {
            public int PartCatalogId { get; set; }
            public int Quantity { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Forbid();

            var orderIds = await _context.ServiceOrders
                .Where(o => o.WorkerId == user.Id)
                .Select(o => o.Id)
                .ToListAsync();

            AvailableTasks = await _context.ServiceTasks
                .Where(t => orderIds.Contains(t.ServiceOrderId))
                .ToListAsync();

            PartCatalog = await _context.PartCatalog.ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (SelectedTaskId == 0)
                return RedirectToPage();

            var task = await _context.ServiceTasks
                .Include(t => t.Parts)
                .FirstOrDefaultAsync(t => t.Id == SelectedTaskId);

            if (task == null)
                return NotFound();

            var order = await _context.ServiceOrders
                .FirstOrDefaultAsync(o => o.Id == task.ServiceOrderId);

            if (order == null)
                return NotFound();

            int totalCost = 0;

            foreach (var input in PartInputs)
            {
                if (input.Quantity <= 0) continue;

                var catalogItem = await _context.PartCatalog.FindAsync(input.PartCatalogId);
                if (catalogItem == null || catalogItem.Stock < input.Quantity)
                    continue;

                int partCost = catalogItem.Price * input.Quantity;

                var part = new Part
                {
                    ServiceOrderId = task.ServiceOrderId,
                    PartCatalogId = catalogItem.Id,
                    Quantity = input.Quantity,
                    Cost = partCost
                };

                order.TotalCost += partCost;

                _context.Parts.Add(part);

                catalogItem.Stock -= input.Quantity;
                totalCost += partCost;
            }

            await _context.SaveChangesAsync();


            if (task.Cost == null) task.Cost = 0;
            task.Cost += totalCost;

            await _context.SaveChangesAsync();

            return RedirectToPage("/ServiceOrders/Details", new { id = task.ServiceOrderId });
        }
    }
}
