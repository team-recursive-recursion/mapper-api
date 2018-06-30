using System;
using System.ComponentModel.DataAnnotations;

namespace Mapper_Api.Models
{
    public class Element
    {
        public enum ElementTypes
        {
            POLYGON = 0,
            POINT = 1
        }

        [Required] [Key] public Guid ElementId { get; set; }

        public ElementTypes ElementType { get; set; }
        public Guid? HoleId { get; set; }
        public Guid CourseId { get; set; }
    }
}
