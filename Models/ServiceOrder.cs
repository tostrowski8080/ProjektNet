using System.ComponentModel.DataAnnotations;

namespace WorkshopManager.Models
{
    public class ServiceOrder
    {
        public enum StatusType
        {
            New,
            InProgress,
            Finished,
            Cancelled
        }

        [Key]
        public int Id { get; set; }

        public int VehicleId { get; set; }

        public DateTime CreationDate { get; set; }

        public StatusType Status { get; set; }

        public string? Description { get; set; } = string.Empty;

        public string? WorkerId { get; set; }

        public int? TotalCost { get; set; } = 0;

        public ICollection<ServiceTask>? Tasks { get; set; } = new List<ServiceTask>();

    }
}
