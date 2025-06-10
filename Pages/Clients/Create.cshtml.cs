using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WorkshopManager.Data;
using WorkshopManager.Models;

namespace WorkshopManager.Pages.Clients
{
    public class CreateModel : PageModel
    {
        private readonly WorkshopDbContext _context;

        public CreateModel(WorkshopDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Client InputClient { get; set; } = new Client();

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            _context.Clients.Add(InputClient);
            await _context.SaveChangesAsync();
            return RedirectToRoleDashboard();
        }

        private IActionResult RedirectToRoleDashboard()
        {
            if (User.IsInRole("Receptionist")) {
                return RedirectToPage("/Dashboard/Receptionist");
            } else if (User.IsInRole("Admin")) {
                return RedirectToPage("/Dashboard/Admin");
            } else if (User.IsInRole("Client")) {
                return RedirectToPage("/Dashboard/Client");
            } else {
                return RedirectToPage("/Index");
            }
        }
    }
}
