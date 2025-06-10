using Microsoft.AspNetCore.Identity;

namespace WorkshopManager.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string UserRole { get; internal set; }
    }
}
