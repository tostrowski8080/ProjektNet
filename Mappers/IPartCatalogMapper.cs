using WorkshopManager.DTOs;
using WorkshopManager.Models;

namespace WorkshopManager.Mappers
{
    public interface IPartCatalogItemMapper
    {
        PartCatalogItemDto ToDto(PartCatalogItem entity);
        PartCatalogItem ToEntity(PartCatalogItemCreateDto dto);
        PartCatalogItem ToEntity(PartCatalogItemUpdateDto dto);
    }
}
