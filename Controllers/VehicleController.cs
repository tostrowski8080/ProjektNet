using Microsoft.AspNetCore.Mvc;
using WorkshopManager.Models;
using WorkshopManager.Services;
using WorkshopManager.DTOs;
using WorkshopManager.Mappers;

namespace WorkshopManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        private readonly IVehicleMapper _vehicleMapper;

        public VehicleController(IVehicleService vehicleService, IVehicleMapper vehicleMapper)
        {
            _vehicleService = vehicleService;
            _vehicleMapper = vehicleMapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateVehicle([FromBody] VehicleCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var vehicle = _vehicleMapper.ToEntity(dto);
            await _vehicleService.AddAsync(vehicle);

            var response = _vehicleMapper.ToDto(vehicle);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehicle(int id)
        {
            var vehicle = await _vehicleService.GetByIdAsync(id);
            if (vehicle == null)
                return NotFound();

            var dto = _vehicleMapper.ToDto(vehicle);
            return Ok(dto);
        }
    }
}