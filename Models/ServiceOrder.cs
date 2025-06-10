using System.ComponentModel.DataAnnotations;

namespace WorkshopManager.Models
{
    public class ServiceOrder
    {
        public enum StatusType
        {
            New,
            InProgress,
            Finished
        }

        [Key]
        public int Id { get; set; }

        public int VehicleId { get; set; }

        public DateTime CreationDate { get; set; }

        public StatusType Status { get; set; }

        public int? WorkerId { get; set; }

        public ICollection<ServiceTask> Tasks { get; set; } = new List<ServiceTask>();

    }
}
