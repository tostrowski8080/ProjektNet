using Riok.Mapperly.Abstractions;
using WorkshopManager.DTOs;
using WorkshopManager.Models;

namespace WorkshopManager.Mappers
{
    [Mapper]
    public partial class ServiceTaskMapper : IServiceTaskMapper
    {
        [MapperIgnoreTarget(nameof(ServiceTask.Id))]
        public partial ServiceTask ToEntity(ServiceTaskCreateDto dto);

        [MapperIgnoreTarget(nameof(ServiceTask.ServiceOrderId))]
        public partial ServiceTask ToEntity(ServiceTaskUpdateDto dto);

        public partial ServiceTaskDto ToDto(ServiceTask entity);
    }
}
