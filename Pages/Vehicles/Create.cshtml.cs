using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WorkshopManager.Data;
using WorkshopManager.Models;

namespace WorkshopManager.Pages.Vehicles
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
        public Vehicle InputVehicle { get; set; } = new Vehicle();

        public List<Client> Clients { get; set; }

        public void OnGet()
        {
            Clients = _context.Clients.ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();
            var client = await _context.Clients.FindAsync(InputVehicle.ClientId);
            InputVehicle.Client = client;
            _context.Vehicles.Add(InputVehicle);
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
