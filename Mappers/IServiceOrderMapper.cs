using WorkshopManager.DTOs;
using WorkshopManager.Models;

namespace WorkshopManager.Mappers
{
    public interface IServiceOrderMapper
    {
        ServiceOrderDto ToDto(ServiceOrder entity);
        ServiceOrder ToEntity(ServiceOrderCreateDto dto);
        ServiceOrder ToEntity(ServiceOrderUpdateDto dto);
    }
}
