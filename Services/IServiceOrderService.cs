using WorkshopManager.DTOs;

namespace WorkshopManager.Services
{
    public interface IServiceOrderService
    {
        Task<IEnumerable<ServiceOrderDto>> GetAllAsync(
            int? vehicleId = null,
            string? status = null);

        Task<ServiceOrderDto?> GetByIdAsync(int id);

        Task<ServiceOrderDto> CreateAsync(ServiceOrderCreateDto dto);

        Task UpdateAsync(ServiceOrderUpdateDto dto);

        Task DeleteAsync(int id);
    }
}
