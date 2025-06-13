using WorkshopManager.DTOs;

namespace WorkshopManager.Services
{
    public interface IServiceTaskService
    {
        Task<IEnumerable<ServiceTaskDto>> GetByOrderIdAsync(int orderId);
        Task<ServiceTaskDto?> GetByIdAsync(int id);
        Task<ServiceTaskDto> CreateAsync(ServiceTaskCreateDto dto);
        Task UpdateAsync(ServiceTaskUpdateDto dto);
        Task DeleteAsync(int id);
    }
}
