using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WorkshopManager.Data;
using WorkshopManager.Models;

namespace WorkshopManager.Pages.ServiceOrders
{
    [Authorize(Roles = "Admin,Receptionist")]
    public class CreateModel : PageModel
    {
        private readonly WorkshopDbContext _context;

        public CreateModel(WorkshopDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public int SelectedVehicleId { get; set; }

        [BindProperty]
        public string? Description { get; set; }

        [BindProperty]
        public string? SelectedWorkerId { get; set; }

        public List<Vehicle> Vehicles { get; set; } = new();
        public List<ApplicationUser> Mechanics { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            Vehicles = await _context.Vehicles
                .Include(v => v.Client)
                .Where(v => v.Client != null && !string.IsNullOrEmpty(v.Client.AccountId))
                .ToListAsync();

            Mechanics = await _context.Users
                .Where(u => u.UserRole == "Mechanic")
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || SelectedVehicleId == 0)
            {
                return Page();
            }

            var order = new ServiceOrder
            {
                VehicleId = SelectedVehicleId,
                CreationDate = DateTime.Now,
                Description = Description,
                Status = ServiceOrder.StatusType.New,
                WorkerId = string.IsNullOrEmpty(SelectedWorkerId) ? null : SelectedWorkerId
            };

            _context.ServiceOrders.Add(order);
            await _context.SaveChangesAsync();

            return RedirectToRoleDashboard();
        }

        private IActionResult RedirectToRoleDashboard()
        {
            if (User.IsInRole("Receptionist"))
            {
                return RedirectToPage("/Dashboard/Receptionist");
            }
            else if (User.IsInRole("Admin"))
            {
                return RedirectToPage("/Dashboard/Admin");
            }
            else if (User.IsInRole("Client"))
            {
                return RedirectToPage("/Dashboard/Client");
            }
            else
            {
                return RedirectToPage("/Index");
            }
        }
    }
}
