using WorkshopManager.DTOs;

namespace WorkshopManager.Services
{
    public interface IPartService
    {
        Task<IEnumerable<PartDto>> GetByOrderIdAsync(int orderId);
        Task<PartDto?> GetByIdAsync(int id);
        Task<PartDto> CreateAsync(PartCreateDto dto);
        Task UpdateAsync(PartUpdateDto dto);
        Task DeleteAsync(int id);
    }
}
