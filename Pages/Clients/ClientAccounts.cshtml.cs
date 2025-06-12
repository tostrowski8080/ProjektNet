using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WorkshopManager.Data;
using WorkshopManager.Models;

namespace WorkshopManager.Pages.Clients
{
    [Authorize(Roles = "Admin,Receptionist")]
    public class ClientAccountsModel : PageModel
    {
        private readonly WorkshopDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ClientAccountsModel(WorkshopDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<Client> UnassignedClients { get; set; } = new();
        public List<ApplicationUser> Users { get; set; } = new();

        [BindProperty]
        public int SelectedClientId { get; set; }

        [BindProperty]
        public string SelectedUserId { get; set; } = string.Empty;

        public async Task OnGetAsync()
        {
            UnassignedClients = await _context.Clients
                .Where(c => string.IsNullOrEmpty(c.AccountId))
                .ToListAsync();

            Users = await _userManager.Users.ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == SelectedClientId);

            if (client == null)
            {
                ModelState.AddModelError("", "Client not found.");
                await OnGetAsync();
                return Page();
            }

            if (!string.IsNullOrEmpty(client.AccountId))
            {
                ModelState.AddModelError("", "This client is already assigned to an account.");
                await OnGetAsync();
                return Page();
            }

            var user = await _userManager.FindByIdAsync(SelectedUserId);

            if (user == null)
            {
                ModelState.AddModelError("", "User not found.");
                await OnGetAsync();
                return Page();
            }

            client.AccountId = user.Id;
            client.user = user;

            _context.Clients.Update(client);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }
    }
}
