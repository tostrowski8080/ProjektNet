namespace WorkshopManager.DTOs
{
    public class ServiceTaskDto
    {
        public int Id { get; set; }
        public int ServiceOrderId { get; set; }
        public string? Description { get; set; }
        public int? Cost { get; set; }
    }
}
