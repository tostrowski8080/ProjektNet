using WorkshopManager.DTOs;

namespace WorkshopManager.Services
{
    public interface IOrderCommentService
    {
        Task<IEnumerable<OrderCommentDto>> GetByOrderIdAsync(int orderId);
        Task<OrderCommentDto?> GetByIdAsync(int id);
        Task<OrderCommentDto> CreateAsync(OrderCommentCreateDto dto);
        Task DeleteAsync(int id);
    }
}
