using System;
using System.ComponentModel.DataAnnotations;

namespace Mapper_Api.Models
{
    public class CourseElement
    {
        [Required] [Key] public Guid CourseElementId { get; set; }

        public Guid? HoleId { get; set; }

        public Guid CourseId { get; set; }
    }
}
