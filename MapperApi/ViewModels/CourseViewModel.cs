using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Mapper_Api.Models;

namespace Mapper_Api.ViewModels
{
    public class CourseViewModel
    {
        public Guid ZoneID { get; set; }
        public string ZoneName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Guid UserId { get; set; }
        public string Info { get; set; }

        public List<Zone> InnerZones { get; set; }
        public List<ElementViewModel> Elements { get; set; }
    }
}
