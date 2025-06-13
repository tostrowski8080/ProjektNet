namespace WorkshopManager.DTOs
{
    public class ServiceOrderDto
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public DateTime CreationDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? WorkerId { get; set; }
        public int TotalCost { get; set; }
    }
}
