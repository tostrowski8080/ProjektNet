using System.ComponentModel.DataAnnotations;


namespace WorkshopManager.Models
{
    public class ServiceTask
    {
        [Key]
        public int Id { get; set; }
        
        public int ServiceOrderId { get; set; }

        public string Description { get; set; }

        public int Cost { get; set; }
    }
}
