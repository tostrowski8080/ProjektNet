using System.ComponentModel.DataAnnotations;

namespace WorkshopManager.DTOs
{
    public class ServiceOrderUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Status { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        public string? WorkerId { get; set; }

        public int? VehicleId { get; set; }
    }
}
