using Riok.Mapperly.Abstractions;
using WorkshopManager.DTOs;
using WorkshopManager.Models;

namespace WorkshopManager.Mappers
{
    [Mapper]
    public partial class PartMapper : IPartMapper
    {
        [MapperIgnoreTarget(nameof(Part.Id))]
        [MapperIgnoreTarget(nameof(Part.Cost))]
        public partial Part ToEntity(PartCreateDto dto);

        [MapperIgnoreTarget(nameof(Part.ServiceOrderId))]
        [MapperIgnoreTarget(nameof(Part.PartCatalogId))]
        [MapperIgnoreTarget(nameof(Part.Cost))]
        public partial Part ToEntity(PartUpdateDto dto);

        public partial PartDto ToDto(Part entity);
    }
}
