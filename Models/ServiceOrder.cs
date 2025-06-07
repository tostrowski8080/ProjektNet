using System.ComponentModel.DataAnnotations;

namespace WorkshopManager.Models
{
    public class ServiceOrder
    {
        public enum StatusType
        {
            New,
            Progress,
            Finished
        }

        [Key]
        public int Id { get; set; }

        public int VehicleId { get; set; }

        public DateTime CreationDate { get; set; }

        public StatusType Status { get; set; }

        public int WorkerId { get; set; }

        public ICollection<Task> Tasks { get; set; } = new List<Task>();

        public ICollection<Part> Parts { get; set; } = new List<Part>();
    }
}
