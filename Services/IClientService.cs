using WorkshopManager.DTOs;
using WorkshopManager.Models;

namespace WorkshopManager.Services
{
    public interface IClientService
    {
        Task<IEnumerable<ClientDto>> GetAllAsync(string? searchTerm = null);
        Task<ClientDto?> GetByIdAsync(int id);
        Task<ClientDto> CreateAsync(ClientCreateDto dto);
        Task UpdateAsync(ClientUpdateDto dto);
        Task DeleteAsync(int id);
        Task AssignToUserAsync(int clientId, string userId);
    }
}
