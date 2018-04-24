using System;
using System.Collections.Generic;

namespace Mapper_Api.Models
{
    public class Hole
    {
        public Guid HoleID { get; set; }
        public List<CoursePolygon> CoursePolygons { get; set; }
        public List<CoursePolygon> CoursePoinrs { get; set; }
    }
}