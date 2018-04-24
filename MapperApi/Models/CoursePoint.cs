using System;
using System.ComponentModel.DataAnnotations;

namespace Mapper_Api.Models
{
    public class Point : CourseElement
    {
        public enum PointTypes
        {
            PIN = 0,
            HOLE = 1,
        }

        [Required] public Guid CourseID { get; set; }
        [Required] public PointTypes Type { get; set; }
        [Required] public DateTime CreatedAt;
        [Required] public DateTime UpdatedAt;
        
    }
}