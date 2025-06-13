using WorkshopManager.DTOs;
using WorkshopManager.Models;

namespace WorkshopManager.Mappers
{
    public interface IOrderCommentMapper
    {
        OrderComment ToEntity(OrderCommentCreateDto dto);
        OrderCommentDto ToDto(OrderComment entity);
    }
}
