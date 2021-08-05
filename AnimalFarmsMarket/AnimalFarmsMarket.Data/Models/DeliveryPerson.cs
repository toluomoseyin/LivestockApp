using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AnimalFarmsMarket.Data.Models
{
    public class DeliveryPerson : BaseEntity
    {
        [Required]
        [StringLength(125)]
        public string Coverage { get; set; }

        [Required]
        [StringLength(125)]
        public string CoverageLocation { get; set; }

        [Required]
        public string NIN { get; set; }
        [Required]
        public string NINTrackingId { get; set; }
        public string AppUserId { get; set; }

        public AppUser AppUser { get; set; }

        public IEnumerable<TrackingHistory> TrackingHistory { get; set; }
        public IEnumerable<DeliveryAssignment> DeliveryAssignments { get; set; }

        public DeliveryPerson()
        {
            TrackingHistory = new HashSet<TrackingHistory>();
            DeliveryAssignments = new HashSet<DeliveryAssignment>();
        }
    }
}
