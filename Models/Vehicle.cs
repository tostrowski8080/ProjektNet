using System.ComponentModel.DataAnnotations;

namespace WorkshopManager.Models
{
    public class Vehicle
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Make { get; set; }

        [Required]
        public string Model { get; set; }

        [Required]
        [MaxLength(12)]
        public string Vin { get; set; }

        [Required]
        public string RegistrationNumber { get; set; }

        [Required]
        public int Year { get; set; }


        public string? PhotoPath { get; set; }

        public int ClientId { get; set; }
        public Client Client { get; set; }

        public ICollection<ServiceOrder> ServiceOrders { get; set; } = new List<ServiceOrder>();
    }
}
