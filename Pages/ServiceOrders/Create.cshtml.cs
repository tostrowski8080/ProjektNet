using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WorkshopManager.Data;
using WorkshopManager.Models;

namespace WorkshopManager.Pages.ServiceOrders
{
    public class CreateModel : PageModel
    {
        private readonly WorkshopDbContext _context;

        public CreateModel(WorkshopDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ServiceOrder InputOrder { get; set; } = new ServiceOrder();

        public List<Vehicle> Vehicles { get; set; }

        public void OnGet()
        {
            Vehicles = _context.Vehicles.ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();
            InputOrder.CreationDate= DateTime.Now;
            InputOrder.Status = ServiceOrder.StatusType.New;
            _context.ServiceOrders.Add(InputOrder);
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
