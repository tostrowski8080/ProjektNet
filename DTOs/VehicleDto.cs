namespace WorkshopManager.DTOs
{
    public class VehicleDto
    {
        public int Id { get; set; }

        public string Make { get; set; } = string.Empty;

        public string Model { get; set; } = string.Empty;

        public string Vin { get; set; } = string.Empty;

        public string RegistrationNumber { get; set; } = string.Empty;

        public int Year { get; set; }

        public string? PhotoPath { get; set; }

        public int ClientId { get; set; }
    }
}