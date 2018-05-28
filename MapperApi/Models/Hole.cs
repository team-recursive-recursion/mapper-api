using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Mapper_Api.Models
{
    public class Hole
    {
        [Key] public Guid HoleID { get; set; }
        [Required] public string Name { get; set; }

        [Required] public Guid CourseId { get; set; }

        public List<CourseElement> CourseElements { get; set; }
    }
}
