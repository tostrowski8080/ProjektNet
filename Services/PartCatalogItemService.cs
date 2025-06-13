using Microsoft.EntityFrameworkCore;
using WorkshopManager.Data;
using WorkshopManager.DTOs;
using WorkshopManager.Mappers;

namespace WorkshopManager.Services
{
    public class PartCatalogItemService : IPartCatalogItemService
    {
        private readonly WorkshopDbContext _context;
        private readonly IPartCatalogItemMapper _mapper;

        public PartCatalogItemService(WorkshopDbContext context, IPartCatalogItemMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PartCatalogItemDto>> GetAllAsync()
        {
            var list = await _context.PartCatalog.ToListAsync();
            return list.Select(_mapper.ToDto);
        }

        public async Task<PartCatalogItemDto?> GetByIdAsync(int id)
        {
            var entity = await _context.PartCatalog.FindAsync(id);
            return entity == null ? null : _mapper.ToDto(entity);
        }

        public async Task<PartCatalogItemDto> CreateAsync(PartCatalogItemCreateDto dto)
        {
            var entity = _mapper.ToEntity(dto);
            _context.PartCatalog.Add(entity);
            await _context.SaveChangesAsync();
            return _mapper.ToDto(entity);
        }

        public async Task UpdateAsync(PartCatalogItemUpdateDto dto)
        {
            var existing = await _context.PartCatalog.FindAsync(dto.Id);
            if (existing == null)
                throw new KeyNotFoundException($"PartCatalogItem {dto.Id} not found.");

            var updated = _mapper.ToEntity(dto);
            _context.Entry(existing).CurrentValues.SetValues(updated);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _context.PartCatalog.FindAsync(id);
            if (existing != null)
            {
                _context.PartCatalog.Remove(existing);
                await _context.SaveChangesAsync();
            }
        }
    }
}
