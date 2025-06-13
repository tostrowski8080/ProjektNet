using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WorkshopManager.DTOs;
using WorkshopManager.Models;
using WorkshopManager.Services;

namespace WorkshopManager.Pages.ServiceTasks
{
    [Authorize(Roles = "Admin,Mechanic")]
    public class CreateModel : PageModel
    {
        private readonly IServiceOrderService _orderService;
        private readonly IServiceTaskService _taskService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CreateModel(
            IServiceOrderService orderService,
            IServiceTaskService taskService,
            UserManager<ApplicationUser> userManager)
        {
            _orderService = orderService;
            _taskService = taskService;
            _userManager = userManager;
        }

        public List<ServiceOrderDto> AssignedOrders { get; set; } = new();

        [BindProperty]
        public ServiceTaskCreateDto Input { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Forbid();

            var all = await _orderService.GetAllAsync();
            AssignedOrders = all
                .Where(o => o.WorkerId == user.Id)
                .ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || Input.ServiceOrderId == 0)
            {
                await OnGetAsync();
                return Page();
            }

            var createdTask = await _taskService.CreateAsync(Input);

            var order = await _orderService.GetByIdAsync(Input.ServiceOrderId);
            if (order != null)
            {
                var updateDto = new ServiceOrderUpdateDto
                {
                    Id = order.Id,
                    Status = order.Status,
                    Description = order.Description,
                    WorkerId = order.WorkerId
                };
                await _orderService.UpdateAsync(updateDto);
            }

            return RedirectToPage("/ServiceOrders/Details", new { id = Input.ServiceOrderId });
        }
    }
}
