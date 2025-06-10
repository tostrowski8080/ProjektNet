using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace WorkshopManager.Models
{
    public class Client
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string AccountId { get; set; } = string.Empty;
        public ApplicationUser? user { get; set; } = null;

        public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    }
}
