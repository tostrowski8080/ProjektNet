using Riok.Mapperly.Abstractions;
using WorkshopManager.DTOs;
using WorkshopManager.Models;

namespace WorkshopManager.Mappers
{
    [Mapper]
    public partial class VehicleMapper : IVehicleMapper
    {
        [MapperIgnoreTarget(nameof(Vehicle.Id))]
        [MapperIgnoreTarget(nameof(Vehicle.PhotoPath))]
        public partial Vehicle ToEntity(VehicleCreateDto dto);

        [MapperIgnoreTarget(nameof(Vehicle.PhotoPath))]
        public partial Vehicle ToEntity(VehicleUpdateDto dto);

        public partial VehicleDto ToDto(Vehicle vehicle);
    }
}
