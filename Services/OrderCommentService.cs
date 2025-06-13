using Microsoft.EntityFrameworkCore;
using WorkshopManager.Data;
using WorkshopManager.DTOs;
using WorkshopManager.Mappers;
using WorkshopManager.Models;

namespace WorkshopManager.Services
{
    public class OrderCommentService : IOrderCommentService
    {
        private readonly WorkshopDbContext _context;
        private readonly IOrderCommentMapper _mapper;

        public OrderCommentService(WorkshopDbContext context, IOrderCommentMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderCommentDto>> GetByOrderIdAsync(int orderId)
        {
            var list = await _context.Set<OrderComment>()
                .Where(c => c.ServiceOrderId == orderId)
                .OrderBy(c => c.CreationDate)
                .ToListAsync();

            return list.Select(_mapper.ToDto);
        }

        public async Task<OrderCommentDto?> GetByIdAsync(int id)
        {
            var entity = await _context.Set<OrderComment>()
                .FindAsync(id);
            return entity == null ? null : _mapper.ToDto(entity);
        }

        public async Task<OrderCommentDto> CreateAsync(OrderCommentCreateDto dto)
        {
            var entity = _mapper.ToEntity(dto);
            entity.CreationDate = DateTime.UtcNow;
            _context.Add(entity);
            await _context.SaveChangesAsync();
            return _mapper.ToDto(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _context.Set<OrderComment>().FindAsync(id);
            if (existing != null)
            {
                _context.Remove(existing);
                await _context.SaveChangesAsync();
            }
        }
    }
}
