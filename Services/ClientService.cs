using WorkshopManager.Data;
using WorkshopManager.Models;
using Microsoft.EntityFrameworkCore;

namespace WorkshopManager.Services
{
    public class ClientService : IClientService
    {
        private readonly WorkshopDbContext _context;

        public ClientService(WorkshopDbContext context) { _context = context; }

        public async Task<IEnumerable<Client>> GetClientsAsync()
        {
            return await _context.Clients.Include(c => c.Vehicles).ToListAsync();
        }

        public async Task<Client?> GetClientByIdAsync(int id)
        {
            return await _context.Clients.Include(c => c.Vehicles).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddClientAsync(Client client)
        {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateClientAsync(Client client)
        {
            _context.Clients.Update(client);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteClientAsync(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client != null)
            {
                _context.Clients.Remove(client);
                await _context.SaveChangesAsync();
            }
        }
        
        public async Task<IEnumerable<Client>> SearchClientsAsync(string searchTerm)
        {
            return await _context.Clients
                .Where(c => c.FirstName.Contains(searchTerm) || c.LastName.Contains(searchTerm))
                .ToListAsync();
        }
    }
}
