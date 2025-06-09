using WorkshopManager.Models;

namespace WorkshopManager.DTOs
{
    public class ClientDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public List<VehicleDto> Vehicles { get; set; } = new();
    }
}
