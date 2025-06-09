using Microsoft.AspNetCore.Identity;

namespace WorkshopManager.Models
{
    public class ApplicationUser : IdentityUser
    {
        public enum Role
        {
            Admin,
            Mechanic,
            Receptionist,
            Client
        }
        public Role UserRole { get; set; }
    }
}
