using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WorkshopManager.DTOs;
using WorkshopManager.Services;

namespace WorkshopManager.Pages.Clients
{
    [Authorize(Roles = "Admin,Receptionist")]
    public class CreateModel : PageModel
    {
        private readonly IClientService _clientService;

        public CreateModel(IClientService clientService)
        {
            _clientService = clientService;
        }

        [BindProperty]
        public ClientCreateDto Input { get; set; } = new();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            await _clientService.CreateAsync(Input);
            return RedirectToRoleDashboard();
        }

        private IActionResult RedirectToRoleDashboard()
        {
            if (User.IsInRole("Receptionist"))
                return RedirectToPage("/Dashboard/Receptionist");
            if (User.IsInRole("Admin"))
                return RedirectToPage("/Dashboard/Admin");
            if (User.IsInRole("Client"))
                return RedirectToPage("/Dashboard/Client");
            return RedirectToPage("/Index");
        }
    }
}
