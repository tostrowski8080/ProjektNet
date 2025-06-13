using System.ComponentModel.DataAnnotations;

namespace WorkshopManager.DTOs
{
    public class OrderCommentCreateDto
    {
        [Required]
        public int ServiceOrderId { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Text { get; set; } = string.Empty;

        [Required]
        public int AuthorId { get; set; }
    }
}
