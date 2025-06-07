using System.ComponentModel.DataAnnotations;

namespace WorkshopManager.Models
{
    public class Part
    {
        [Key]
        public int Id { get; set; }

        public int ServiceOrderId { get; set; }

        public int PartCatalogId { get; set; }

        public int Quantity { get; set; }

        public int Cost { get; set; }
    }
}
