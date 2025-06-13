namespace WorkshopManager.DTOs
{
    public class PartCatalogItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Price { get; set; }
        public int Stock { get; set; }
    }
}
