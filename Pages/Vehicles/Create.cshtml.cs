using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WorkshopManager.DTOs;
using WorkshopManager.Mappers;
using WorkshopManager.Models;
using WorkshopManager.Services;

namespace WorkshopManager.Pages.Vehicles
{
    [Authorize(Roles = "Admin,Receptionist")]
    public class CreateModel : PageModel
    {
        private readonly IClientService _clientService;
        private readonly IVehicleService _vehicleService;
        private readonly IVehicleMapper _vehicleMapper;

        public CreateModel(
            IClientService clientService,
            IVehicleService vehicleService,
            IVehicleMapper vehicleMapper)
        {
            _clientService = clientService;
            _vehicleService = vehicleService;
            _vehicleMapper = vehicleMapper;
        }

        [BindProperty]
        public VehicleCreateDto InputVehicle { get; set; } = new();

        public List<SelectListItem> Clients { get; set; } = new();

        public async Task OnGetAsync()
        {
            var clients = await _clientService.GetAllAsync();
            Clients = clients
                .Select(c => new SelectListItem(
                    text: $"{c.FirstName} {c.LastName}",
                    value: c.Id.ToString()))
                .ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            var vehicleEntity = _vehicleMapper.ToEntity(InputVehicle);
            await _vehicleService.AddAsync(vehicleEntity);

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