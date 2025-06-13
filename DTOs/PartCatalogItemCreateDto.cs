using System.ComponentModel.DataAnnotations;

namespace WorkshopManager.DTOs
{
    public class PartCatalogItemCreateDto
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Range(0, int.MaxValue)]
        public int Price { get; set; }

        [Range(0, int.MaxValue)]
        public int Stock { get; set; }
    }
}
