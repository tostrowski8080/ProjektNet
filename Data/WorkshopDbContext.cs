using Azure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WorkshopManager.Models;

namespace WorkshopManager.Data
{
    public class WorkshopDbContext : IdentityDbContext<ApplicationUser>
    {
        public WorkshopDbContext(DbContextOptions<WorkshopDbContext> options)
            : base(options){ }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<ServiceOrder> ServiceOrders { get; set; }
        public DbSet<ServiceTask> ServiceTasks { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<OrderComment> OrderComments { get; set; }
        public DbSet<PartCatalogItem> PartCatalog { get; set; }
    }
}
