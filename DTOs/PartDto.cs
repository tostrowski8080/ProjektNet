namespace WorkshopManager.DTOs
{
    public class PartDto
    {
        public int Id { get; set; }
        public int ServiceOrderId { get; set; }
        public int PartCatalogId { get; set; }
        public int Quantity { get; set; }
        public int Cost { get; set; }
    }
}
