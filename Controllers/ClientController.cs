using Microsoft.AspNetCore.Mvc;
using WorkshopManager.DTOs;
using WorkshopManager.Mappers;
using WorkshopManager.Models;

namespace WorkshopManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IClientMapper _clientMapper;

        public ClientController(IClientMapper clientMapper)
        {
            _clientMapper = clientMapper;
        }

        [HttpPost]
        public IActionResult CreateClient([FromBody] ClientCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var client = _clientMapper.ToEntity(dto);
            client.Id = 1;

            var response = _clientMapper.ToDto(client);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetClient(int id)
        {
            var client = new Client
            {
                Id = id,
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "1234567890",
                Email = "john@example.com",
                Vehicles = new List<Vehicle>()
            };

            var dto = _clientMapper.ToDto(client);
            return Ok(dto);
        }
    }
}
