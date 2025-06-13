using WorkshopManager.DTOs;
using WorkshopManager.Models;

namespace WorkshopManager.Mappers
{
    public interface IServiceTaskMapper
    {
        ServiceTaskDto ToDto(ServiceTask entity);
        ServiceTask ToEntity(ServiceTaskCreateDto dto);
        ServiceTask ToEntity(ServiceTaskUpdateDto dto);
    }
}
