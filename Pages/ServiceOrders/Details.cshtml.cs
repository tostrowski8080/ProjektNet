using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WorkshopManager.DTOs;
using WorkshopManager.Services;
using WorkshopManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace WorkshopManager.Pages.ServiceOrders
{
    [Authorize(Roles = "Admin,Mechanic")]
    public class DetailsModel : PageModel
    {
        private readonly IServiceOrderService _orderService;
        private readonly UserManager<ApplicationUser> _userManager;

        public DetailsModel(IServiceOrderService orderService, UserManager<ApplicationUser> userManager)
        {
            _orderService = orderService;
            _userManager = userManager;
        }

        public List<ServiceOrderDto> Orders { get; set; } = new();

        [BindProperty]
        public Dictionary<int, string> UpdatedStatuses { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var mechanicId = _userManager.GetUserId(User);
            if (mechanicId == null)
                return Unauthorized();

            var all = await _orderService.GetAllAsync();
            Orders = all
                .Where(o => o.WorkerId == mechanicId)
                .ToList();

            foreach (var o in Orders)
                UpdatedStatuses[o.Id] = o.Status;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var mechanicId = User.Identity?.Name;
            if (mechanicId == null)
                return Unauthorized();

            foreach (var kv in UpdatedStatuses)
            {
                var dto = new ServiceOrderUpdateDto
                {
                    Id = kv.Key,
                    Status = kv.Value
                };
                await _orderService.UpdateAsync(dto);
            }

            return RedirectToPage();
        }
    }
}
