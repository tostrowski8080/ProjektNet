using Riok.Mapperly.Abstractions;
using WorkshopManager.DTOs;
using WorkshopManager.Models;

namespace WorkshopManager.Mappers
{
    [Mapper]
    public partial class ServiceOrderMapper : IServiceOrderMapper
    {
        [MapperIgnoreTarget(nameof(ServiceOrder.Id))]
        [MapperIgnoreTarget(nameof(ServiceOrder.CreationDate))]
        [MapperIgnoreTarget(nameof(ServiceOrder.TotalCost))]
        public partial ServiceOrder ToEntity(ServiceOrderCreateDto dto);

        [MapperIgnoreTarget(nameof(ServiceOrder.CreationDate))]
        [MapperIgnoreTarget(nameof(ServiceOrder.TotalCost))]
        public partial ServiceOrder ToEntity(ServiceOrderUpdateDto dto);

        public partial ServiceOrderDto ToDto(ServiceOrder entity);
    }
}
