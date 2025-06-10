using System.ComponentModel.DataAnnotations;


namespace WorkshopManager.Models
{
    public class ServiceTask
    {
        [Key]
        public int Id { get; set; }
        
        public int ServiceOrderId { get; set; }

        public string Description { get; set; } = string.Empty;

        public int Cost { get; set; }

        public ICollection<Part> Parts { get; set; } = new List<Part>();
    }
}
