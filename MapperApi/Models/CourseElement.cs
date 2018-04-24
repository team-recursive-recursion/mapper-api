using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mapper_Api.Models
{
    public class CourseElement
    {
        [Required] [Key] public Guid CourseElementID { get; set; }
        [Required] public Guid GolfCourseID { get; set; }
        public Guid? HoleID { get; set; }
    }
}