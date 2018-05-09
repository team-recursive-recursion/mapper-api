using System;
using System.Collections.Generic;

namespace Mapper_Api.Models
{
    public class Hole
    {
        public Guid HoleID { get; set; }
        public string Name { get; set; }

        public Guid CourseId { get; set; }
        public GolfCourse GolfCourse { get; set; }

        public List<CourseElement> CourseElements { get; set; }
    }
}