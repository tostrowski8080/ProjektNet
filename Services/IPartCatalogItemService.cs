using WorkshopManager.DTOs;

namespace WorkshopManager.Services
{
    public interface IPartCatalogItemService
    {
        Task<IEnumerable<PartCatalogItemDto>> GetAllAsync();
        Task<PartCatalogItemDto?> GetByIdAsync(int id);
        Task<PartCatalogItemDto> CreateAsync(PartCatalogItemCreateDto dto);
        Task UpdateAsync(PartCatalogItemUpdateDto dto);
        Task DeleteAsync(int id);
    }
}
