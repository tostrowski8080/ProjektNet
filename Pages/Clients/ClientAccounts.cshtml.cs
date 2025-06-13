using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WorkshopManager.DTOs;
using WorkshopManager.Services;
using WorkshopManager.Models;

namespace WorkshopManager.Pages.Clients
{
    [Authorize(Roles = "Admin,Receptionist")]
    public class ClientAccountsModel : PageModel
    {
        private readonly IClientService _clientService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ClientAccountsModel(
            IClientService clientService,
            UserManager<ApplicationUser> userManager)
        {
            _clientService = clientService;
            _userManager = userManager;
        }

        // Do wype³nienia w OnGet
        public List<ClientDto> UnassignedClients { get; set; } = new();
        public List<IdentityUserDto> Users { get; set; } = new();

        [BindProperty]
        public int SelectedClientId { get; set; }

        [BindProperty]
        public string SelectedUserId { get; set; } = string.Empty;

        public async Task OnGetAsync()
        {
            UnassignedClients = (await _clientService.GetAllAsync())
                                .Where(c => string.IsNullOrEmpty(c.AccountId))
                                .ToList();

            Users = _userManager.Users
                .Select(u => new IdentityUserDto { Id = u.Id, UserName = u.UserName!, Email = u.Email! })
                .ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            await _clientService.AssignToUserAsync(SelectedClientId, SelectedUserId);

            if (User.IsInRole("Receptionist"))
                return RedirectToPage("/Dashboard/Receptionist");
            if (User.IsInRole("Admin"))
                return RedirectToPage("/Dashboard/Admin");

            return RedirectToPage("/Index");
        }
    }

    public class IdentityUserDto
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
