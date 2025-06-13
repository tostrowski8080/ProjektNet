using WorkshopManager.DTOs;
using WorkshopManager.Models;

namespace WorkshopManager.Mappers
{
    public interface IVehicleMapper
    {
        VehicleDto ToDto(Vehicle vehicle);
        Vehicle ToEntity(VehicleCreateDto dto);
        Vehicle ToEntity(VehicleUpdateDto dto);
    }
}
