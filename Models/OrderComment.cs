using System.ComponentModel.DataAnnotations;

namespace WorkshopManager.Models
{
    public class OrderComment
    {
        [Key]
        public int Id { get; set; }

        public int ServiceOrderId { get; set; }

        public int AuthorId { get; set; }

        public string Text { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
