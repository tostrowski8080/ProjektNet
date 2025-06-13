using Riok.Mapperly.Abstractions;
using WorkshopManager.DTOs;
using WorkshopManager.Models;

namespace WorkshopManager.Mappers
{
    [Mapper]
    public partial class OrderCommentMapper : IOrderCommentMapper
    {
        [MapperIgnoreTarget(nameof(OrderComment.Id))]
        [MapperIgnoreTarget(nameof(OrderComment.CreationDate))]
        public partial OrderComment ToEntity(OrderCommentCreateDto dto);

        public partial OrderCommentDto ToDto(OrderComment entity);
    }
}
