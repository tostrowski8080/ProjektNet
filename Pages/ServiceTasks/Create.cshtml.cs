using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WorkshopManager.Data;
using WorkshopManager.Models;

namespace WorkshopManager.Pages.ServiceTasks
{
    [Authorize(Roles = "Admin,Mechanic")]
    public class CreateModel : PageModel
    {
        private readonly WorkshopDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CreateModel(WorkshopDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public int SelectedOrderId { get; set; }

        [BindProperty]
        public string? Description { get; set; }

        [BindProperty]
        public int? Cost { get; set; }

        public List<ServiceOrder> AssignedOrders { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            AssignedOrders = await _context.ServiceOrders.Where(so => so.WorkerId == user.Id).ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || SelectedOrderId == 0)
                return Page();

            var task = new ServiceTask
            {
                ServiceOrderId = SelectedOrderId,
                Description = Description,
                Cost = Cost
            };

            var serviceOrder = await _context.ServiceOrders.FindAsync(SelectedOrderId);
            if (serviceOrder == null)
                return NotFound();
            serviceOrder.TotalCost += Cost;

            _context.ServiceTasks.Add(task);
            await _context.SaveChangesAsync();

            return RedirectToPage("/ServiceOrders/Details", new { id = SelectedOrderId });
        }
    }
}
