using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mapper_Api.Models
{
    public class Zone
    {
        [Key] public Guid ZoneID { get; set; }
        [Required] public string ZoneName { get; set; }
        [Required] public Guid UserId { get; set; }
        [Required] public DateTime CreatedAt { get; set; }
        [Required] public DateTime UpdatedAt { get; set; }

        // Optional
        public string Info { get; set; }
        public Guid ParentZoneID { get; set; }

        // Relations
        public List<Zone> InnerZones { get; set; }
        public List<Element> Elements { get; set; }
    }
}
