using Riok.Mapperly.Abstractions;
using WorkshopManager.DTOs;
using WorkshopManager.Models;

namespace WorkshopManager.Mappers
{
    [Mapper]
    public partial class PartCatalogItemMapper : IPartCatalogItemMapper
    {
        [MapperIgnoreTarget(nameof(PartCatalogItem.Id))]
        public partial PartCatalogItem ToEntity(PartCatalogItemCreateDto dto);

        public partial PartCatalogItem ToEntity(PartCatalogItemUpdateDto dto);

        public partial PartCatalogItemDto ToDto(PartCatalogItem entity);
    }
}
