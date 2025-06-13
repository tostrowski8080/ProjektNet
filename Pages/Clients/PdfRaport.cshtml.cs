using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WorkshopManager.Services;
using WorkshopManager.PdfReports;

namespace WorkshopManager.Pages.Clients
{
    [Authorize(Roles = "Admin,Receptionist")]
    public class RepairReportModel : PageModel
    {
        private readonly IClientService _clientService;

        public RepairReportModel(IClientService clientService)
        {
            _clientService = clientService;
        }

        [BindProperty(SupportsGet = true)]
        public int SelectedClientId { get; set; }

        public List<SelectListItem> ClientList { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var clients = await _clientService.GetAllAsync();
            ClientList = clients.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = $"{c.FirstName} {c.LastName}"
            }).ToList();

            if (SelectedClientId > 0)
            {
                var fullClient = await _clientService.GetCustomerWithFullDetailsAsync(SelectedClientId);
                if (fullClient == null) return NotFound();

                var pdf = ClientReport.Generate(fullClient);
                return File(pdf, "application/pdf", $"Raport_{fullClient.LastName}.pdf");
            }

            return Page();
        }
    }
}
