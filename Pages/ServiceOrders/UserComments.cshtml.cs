using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WorkshopManager.DTOs;
using WorkshopManager.Services;

namespace WorkshopManager.Pages.ServiceOrders
{
    public class UserCommentsModel : PageModel
    {
        private readonly IClientService _clientService;
        private readonly IServiceOrderService _orderService;
        private readonly IOrderCommentService _commentService;

        public UserCommentsModel(
            IClientService clientService,
            IServiceOrderService orderService,
            IOrderCommentService commentService)
        {
            _clientService = clientService;
            _orderService = orderService;
            _commentService = commentService;
        }

        public List<ServiceOrderDto> Orders { get; set; } = new();

        public Dictionary<int, List<OrderCommentDto>> CommentsByOrder { get; set; } = new();

        [BindProperty]
        public Dictionary<int, string> NewComments { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var accountId = User.Identity?.Name;
            if (string.IsNullOrEmpty(accountId))
                return Unauthorized();

            // Pobierz klienta po przypisanym AccountId
            var clients = await _clientService.GetAllAsync();
            var client = clients.FirstOrDefault(c => c.AccountId == accountId);
            if (client == null)
                return NotFound("Client not found.");

            var allOrders = await _orderService.GetAllAsync();
            Orders = allOrders
                .Where(o => client.Vehicles.Any(v => v.Id == o.VehicleId))
                .ToList();

            CommentsByOrder = new Dictionary<int, List<OrderCommentDto>>();
            foreach (var o in Orders)
            {
                var comms = (await _commentService.GetByOrderIdAsync(o.Id)).ToList();
                CommentsByOrder[o.Id] = comms;
                NewComments[o.Id] = string.Empty;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var accountId = User.Identity?.Name;
            if (string.IsNullOrEmpty(accountId))
                return Unauthorized();

            var clients = await _clientService.GetAllAsync();
            var client = clients.FirstOrDefault(c => c.AccountId == accountId);
            if (client == null)
                return NotFound("Client not found.");

            foreach (var kv in NewComments)
            {
                var text = kv.Value?.Trim();
                if (string.IsNullOrEmpty(text))
                    continue;

                var createDto = new OrderCommentCreateDto
                {
                    ServiceOrderId = kv.Key,
                    AuthorId = client.Id,
                    Text = text
                };
                await _commentService.CreateAsync(createDto);
            }

            return RedirectToPage();
        }
    }
}

