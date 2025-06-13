using System.ComponentModel.DataAnnotations;

namespace WorkshopManager.DTOs
{
    public class PartCreateDto
    {
        [Required]
        public int ServiceOrderId { get; set; }

        [Required]
        public int PartCatalogId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
