using WorkshopManager.DTOs;
using WorkshopManager.Models;

namespace WorkshopManager.Mappers
{
    public interface IPartMapper
    {
        PartDto ToDto(Part entity);
        Part ToEntity(PartCreateDto dto);
        Part ToEntity(PartUpdateDto dto);
    }
}
