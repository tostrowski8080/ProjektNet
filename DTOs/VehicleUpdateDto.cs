using System.ComponentModel.DataAnnotations;

namespace WorkshopManager.DTOs
{
    public class VehicleUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Make { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Model { get; set; } = string.Empty;

        [Required]
        [StringLength(17, MinimumLength = 11)]
        public string Vin { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string RegistrationNumber { get; set; } = string.Empty;

        [Required]
        [Range(1900, 2100)]
        public int Year { get; set; }

        public IFormFile? Photo { get; set; }

        [Required]
        public int ClientId { get; set; }
    }
}
