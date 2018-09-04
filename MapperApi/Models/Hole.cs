using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Mapper_Api.Models
{
    public class Hole
    {
        [Key] public Guid HoleId { get; set; }
        [Required] public string Name { get; set; }
        public string Info { get; set; }

        [Required] public Guid CourseId { get; set; }

        public List<Element> Elements { get; set; }
    }
}
