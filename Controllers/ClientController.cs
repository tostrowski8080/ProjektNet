using Microsoft.AspNetCore.Mvc;
using WorkshopManager.DTOs;
using WorkshopManager.Mappers;
using WorkshopManager.Models;
using WorkshopManager.Services;
using WorkshopManager.PdfReports;

namespace WorkshopManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IClientMapper _clientMapper;

        private readonly IClientService _clientService;

        public ClientController(IClientService clientService, IClientMapper clientMapper)
        {
            _clientService = clientService;
            _clientMapper = clientMapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateClient([FromBody] ClientCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _clientService.CreateAsync(dto);
            return Ok(dto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClient(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var client = await _clientService.GetByIdAsync(id);
            return Ok(client);
        }

        [HttpGet("{id}/repair-report")]
        public async Task<IActionResult> GenerateRepairReport(int id)
        {
            var customer = await _clientService.GetCustomerWithFullDetailsAsync(id);
            if (customer == null)
                return NotFound();

            var pdf = ClientReport.Generate(customer);
            return File(pdf, "application/pdf", $"Raport_{customer.LastName}.pdf");
        }
    }
}
