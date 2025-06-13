using System.ComponentModel.DataAnnotations;

namespace WorkshopManager.DTOs
{
    public class ServiceOrderCreateDto
    {
        [Required]
        public int VehicleId { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        public string? WorkerId { get; set; }
    }
}
