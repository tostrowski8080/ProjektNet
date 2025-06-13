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
    public class AddPartModel : PageModel
    {
        private readonly IServiceTaskService _taskService;
        private readonly IPartCatalogItemService _catalogService;
        private readonly IPartService _partService;
        private readonly IServiceOrderService _orderService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AddPartModel(
            IServiceTaskService taskService,
            IPartCatalogItemService catalogService,
            IPartService partService,
            IServiceOrderService orderService,
            UserManager<ApplicationUser> userManager)
        {
            _taskService = taskService;
            _catalogService = catalogService;
            _partService = partService;
            _orderService = orderService;
            _userManager = userManager;
        }

        public List<ServiceTaskDto> AvailableTasks { get; set; } = new();

        public List<PartCatalogItemDto> PartCatalog { get; set; } = new();

        [BindProperty]
        public int SelectedTaskId { get; set; }

        [BindProperty]
        public List<PartInputModel> PartInputs { get; set; } = new();

        public class PartInputModel
        {
            public int PartCatalogId { get; set; }
            public int Quantity { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Forbid();

            var allOrders = await _orderService.GetAllAsync();
            var myOrders = allOrders.Where(o => o.WorkerId == user.Id).Select(o => o.Id);

            var tasks = new List<ServiceTaskDto>();
            foreach (var oid in myOrders)
            {
                tasks.AddRange(await _taskService.GetByOrderIdAsync(oid));
            }
            AvailableTasks = tasks;

            PartCatalog = (await _catalogService.GetAllAsync()).ToList();

            PartInputs = PartCatalog
                .Select(i => new PartInputModel { PartCatalogId = i.Id, Quantity = 0 })
                .ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (SelectedTaskId == 0)
                return RedirectToPage();

            int sumTaskCost = 0;
            int orderId = 0;

            foreach (var input in PartInputs)
            {
                if (input.Quantity <= 0) continue;

                var catalog = await _catalogService.GetByIdAsync(input.PartCatalogId);
                if (catalog == null || catalog.Stock < input.Quantity) continue;

                var partDto = new PartCreateDto
                {
                    ServiceOrderId = 0,
                    PartCatalogId = input.PartCatalogId,
                    Quantity = input.Quantity
                };
                var task = await _taskService.GetByIdAsync(SelectedTaskId);
                if (task == null) return NotFound();
                partDto.ServiceOrderId = task.ServiceOrderId;

                var created = await _partService.CreateAsync(partDto);
                sumTaskCost += created.Cost;
                orderId = created.ServiceOrderId;

                var updateCatalog = new PartCatalogItemUpdateDto
                {
                    Id = catalog.Id,
                    Name = catalog.Name,
                    Price = catalog.Price,
                    Stock = catalog.Stock - input.Quantity
                };
                await _catalogService.UpdateAsync(updateCatalog);
            }

            if (sumTaskCost > 0)
            {
                var origTask = await _taskService.GetByIdAsync(SelectedTaskId);
                var updateTask = new ServiceTaskUpdateDto
                {
                    Id = SelectedTaskId,
                    Description = origTask?.Description,
                    Cost = (origTask?.Cost ?? 0) + sumTaskCost
                };
                await _taskService.UpdateAsync(updateTask);
            }

            if (orderId != 0)
            {
                var origOrder = (await _orderService.GetByIdAsync(orderId))!;
                var updateOrder = new ServiceOrderUpdateDto
                {
                    Id = orderId,
                    Status = origOrder.Status,
                    Description = origOrder.Description,
                    WorkerId = origOrder.WorkerId
                };
                await _orderService.UpdateAsync(updateOrder);
            }

            return RedirectToPage("/ServiceOrders/Details", new { id = orderId });
        }
    }
}