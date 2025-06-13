using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WorkshopManager.DTOs;
using WorkshopManager.Services;
using WorkshopManager.Models;

namespace WorkshopManager.Pages.ServiceOrders
{
    [Authorize(Roles = "Admin,Receptionist")]
    public class CreateModel : PageModel
    {
        private readonly IServiceOrderService _orderService;
        private readonly IVehicleService _vehicleService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CreateModel(
            IServiceOrderService orderService,
            IVehicleService vehicleService,
            UserManager<ApplicationUser> userManager)
        {
            _orderService = orderService;
            _vehicleService = vehicleService;
            _userManager = userManager;
        }

        public List<Vehicle> Vehicles { get; set; } = new();
        public List<ApplicationUser> Mechanics { get; set; } = new();

        [BindProperty]
        public ServiceOrderCreateDto Input { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var all = await _vehicleService.GetAllAsync();
            Vehicles = all.Where(v => v.Client != null && !string.IsNullOrEmpty(v.Client.AccountId)).ToList();

            Mechanics = (await _userManager.GetUsersInRoleAsync("Mechanic")).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || Input.VehicleId == 0)
            {
                await OnGetAsync();
                return Page();
            }

            if (string.IsNullOrWhiteSpace(Input.WorkerId))
                Input.WorkerId = null;

            await _orderService.CreateAsync(Input);
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
