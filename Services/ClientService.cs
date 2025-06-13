using WorkshopManager.Data;
using WorkshopManager.Models;
using Microsoft.EntityFrameworkCore;
using WorkshopManager.DTOs;
using WorkshopManager.Mappers;

namespace WorkshopManager.Services
{
    public class ClientService : IClientService
    {
        private readonly WorkshopDbContext _context;
        private readonly IClientMapper _mapper;

        public ClientService(WorkshopDbContext context, IClientMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ClientDto>> GetAllAsync(string? searchTerm = null)
        {
            var query = _context.Clients
                .Include(c => c.Vehicles)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(c =>
                    c.FirstName.Contains(searchTerm) ||
                    c.LastName.Contains(searchTerm) ||
                    c.Email.Contains(searchTerm));
            }

            var clients = await query.ToListAsync();
            return clients.Select(_mapper.ToDto);
        }

        public async Task<ClientDto?> GetByIdAsync(int id)
        {
            var client = await _context.Clients
                .Include(c => c.Vehicles)
                .FirstOrDefaultAsync(c => c.Id == id);
            return client is null ? null : _mapper.ToDto(client);
        }

        public async Task<ClientDto> CreateAsync(ClientCreateDto dto)
        {
            var entity = _mapper.ToEntity(dto);
            _context.Clients.Add(entity);
            await _context.SaveChangesAsync();
            return _mapper.ToDto(entity);
        }

        public async Task UpdateAsync(ClientUpdateDto dto)
        {
            var existing = await _context.Clients.FindAsync(dto.Id);
            if (existing is null)
                throw new KeyNotFoundException($"Client with ID {dto.Id} not found.");

            var updated = _mapper.ToEntity(dto);
            updated.Vehicles = existing.Vehicles;
            updated.AccountId = existing.AccountId;
            _context.Entry(existing).CurrentValues.SetValues(updated);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client != null)
            {
                _context.Clients.Remove(client);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AssignToUserAsync(int clientId, string userId)
        {
            var client = await _context.Clients
                .FirstOrDefaultAsync(c => c.Id == clientId);

            if (client == null)
                throw new KeyNotFoundException($"Client with ID {clientId} not found.");
            client.AccountId = userId;

            _context.Clients.Update(client);
            await _context.SaveChangesAsync();
        }
    }
}
