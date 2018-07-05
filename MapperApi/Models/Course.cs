using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mapper_Api.Models
{
    public class Course
    {
        [Key] public Guid CourseId { get; set; }
        [Required] public string CourseName { get; set; }
        [Required] public DateTime CreatedAt { get; set; }
        [Required] public DateTime UpdatedAt { get; set; }

        [Required] public Guid UserId { get; set; }

        public List<Hole> Holes { get; set; }
        public List<Element> Elements { get; set; }
    }
}
