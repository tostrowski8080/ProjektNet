using WorkshopManager.Models;

namespace WorkshopManager.Services
{
    public interface IClientService
    {
        Task<IEnumerable<Client>> GetAllClientsAsync();
        Task<Client?> GetClientByIdAsync(int id);
        Task AddClientAsync(Client client);
        Task UpdateClientAsync(Client client);
        Task DeleteClientAsync(int id);
        Task<IEnumerable<Client>> SearchClientsAsync(string searchTerm);
    }
}
