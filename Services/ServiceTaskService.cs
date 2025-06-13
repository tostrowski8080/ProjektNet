using WorkshopManager.Data;
using WorkshopManager.DTOs;
using WorkshopManager.Mappers;
using Microsoft.EntityFrameworkCore;

namespace WorkshopManager.Services
{
    public class ServiceTaskService : IServiceTaskService
    {
        private readonly WorkshopDbContext _context;
        private readonly IServiceTaskMapper _mapper;

        public ServiceTaskService(WorkshopDbContext context, IServiceTaskMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ServiceTaskDto>> GetByOrderIdAsync(int orderId)
        {
            var list = await _context.ServiceTasks
                .Where(t => t.ServiceOrderId == orderId)
                .ToListAsync();
            return list.Select(_mapper.ToDto);
        }

        public async Task<ServiceTaskDto?> GetByIdAsync(int id)
        {
            var entity = await _context.ServiceTasks.FindAsync(id);
            return entity == null ? null : _mapper.ToDto(entity);
        }

        public async Task<ServiceTaskDto> CreateAsync(ServiceTaskCreateDto dto)
        {
            var entity = _mapper.ToEntity(dto);
            _context.ServiceTasks.Add(entity);
            await _context.SaveChangesAsync();
            return _mapper.ToDto(entity);
        }

        public async Task UpdateAsync(ServiceTaskUpdateDto dto)
        {
            var existing = await _context.ServiceTasks.FindAsync(dto.Id);
            if (existing == null)
                throw new KeyNotFoundException($"ServiceTask {dto.Id} not found.");

            var updated = _mapper.ToEntity(dto);
            updated.ServiceOrderId = existing.ServiceOrderId;
            _context.Entry(existing).CurrentValues.SetValues(updated);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _context.ServiceTasks.FindAsync(id);
            if (existing != null)
            {
                _context.ServiceTasks.Remove(existing);
                await _context.SaveChangesAsync();
            }
        }
    }
}
