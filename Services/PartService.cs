using Microsoft.EntityFrameworkCore;
using WorkshopManager.Data;
using WorkshopManager.DTOs;
using WorkshopManager.Mappers;

namespace WorkshopManager.Services
{
    public class PartService : IPartService
    {
        private readonly WorkshopDbContext _context;
        private readonly IPartMapper _mapper;

        public PartService(WorkshopDbContext context, IPartMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PartDto>> GetByOrderIdAsync(int orderId)
        {
            var list = await _context.Parts
                .Where(p => p.ServiceOrderId == orderId)
                .ToListAsync();
            return list.Select(_mapper.ToDto);
        }

        public async Task<PartDto?> GetByIdAsync(int id)
        {
            var entity = await _context.Parts.FindAsync(id);
            return entity == null ? null : _mapper.ToDto(entity);
        }

        public async Task<PartDto> CreateAsync(PartCreateDto dto)
        {
            var catalog = await _context.PartCatalog.FindAsync(dto.PartCatalogId)
                ?? throw new KeyNotFoundException($"Catalog item {dto.PartCatalogId} not found.");

            var entity = _mapper.ToEntity(dto);
            entity.Cost = dto.Quantity * catalog.Price;
            _context.Parts.Add(entity);
            await _context.SaveChangesAsync();
            return _mapper.ToDto(entity);
        }

        public async Task UpdateAsync(PartUpdateDto dto)
        {
            var existing = await _context.Parts.FindAsync(dto.Id);
            if (existing == null)
                throw new KeyNotFoundException($"Part {dto.Id} not found.");

            existing.Quantity = dto.Quantity;
            var catalog = await _context.PartCatalog.FindAsync(existing.PartCatalogId)
                ?? throw new KeyNotFoundException($"Catalog item {existing.PartCatalogId} not found.");
            existing.Cost = dto.Quantity * catalog.Price;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _context.Parts.FindAsync(id);
            if (existing != null)
            {
                _context.Parts.Remove(existing);
                await _context.SaveChangesAsync();
            }
        }
    }
}
