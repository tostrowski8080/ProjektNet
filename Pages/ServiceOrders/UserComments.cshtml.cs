using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WorkshopManager.Data;
using WorkshopManager.DTOs;
using WorkshopManager.Models;
using WorkshopManager.Services;

namespace WorkshopManager.Pages.ServiceOrders
{
    public class UserCommentsModel : PageModel
    {
        private readonly IClientService _clientService;
        private readonly IServiceOrderService _orderService;
        private readonly IOrderCommentService _commentService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly WorkshopDbContext _context;


        public UserCommentsModel(IClientService clientService,IServiceOrderService orderService,IOrderCommentService commentService,UserManager<ApplicationUser> userManager, WorkshopDbContext context)
        {
            _clientService = clientService;
            _orderService = orderService;
            _commentService = commentService;
            _userManager = userManager;
            _context = context;
        }

        public List<ServiceOrderDto> Orders { get; set; } = new();

        public Dictionary<int, List<OrderCommentDto>> CommentsByOrder { get; set; } = new();

        [BindProperty]
        public Dictionary<int, string> NewComments { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var accountId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(accountId))
                return Unauthorized();

            var clients = await _clientService.GetAllAsync();
            var rawClients = await _context.Clients.Include(c => c.Vehicles).ToListAsync();
            var client = rawClients.FirstOrDefault(c => c.AccountId == accountId);
            if (client == null || client.Vehicles == null || client.Vehicles.Count == 0)
            {
                Console.WriteLine("Client not found or has no vehicles.");
                return NotFound("Client not found or has no vehicles.");
            }

            var clientVehicleIds = client.Vehicles.Select(v => v.Id).ToHashSet();
            Console.WriteLine($"Client ID: {client.Id}, Vehicles: {string.Join(",", clientVehicleIds)}");

            var allOrders = await _orderService.GetAllAsync();

            Orders = allOrders
                .Where(o => clientVehicleIds.Contains(o.VehicleId))
                .ToList();

            Console.WriteLine($"Orders found: {Orders.Count}");

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
            var accountId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(accountId))
                return Unauthorized();

            var clients = await _clientService.GetAllAsync();
            var rawClients = await _context.Clients.Include(c => c.Vehicles).ToListAsync();
            var client = rawClients.FirstOrDefault(c => c.AccountId == accountId);
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

