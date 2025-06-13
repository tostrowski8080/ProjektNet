using System.ComponentModel.DataAnnotations;

namespace WorkshopManager.DTOs
{
    public class ServiceTaskUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [Range(0, int.MaxValue)]
        public int? Cost { get; set; }
    }
}
