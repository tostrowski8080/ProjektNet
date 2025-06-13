using System.ComponentModel.DataAnnotations;

namespace WorkshopManager.DTOs
{
    public class PartUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
