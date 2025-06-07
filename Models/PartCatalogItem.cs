using System.ComponentModel.DataAnnotations;

namespace WorkshopManager.Models
{
    public class PartCatalogItem
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int Price { get; set; }

        public int Stock { get; set; }
    }
}
