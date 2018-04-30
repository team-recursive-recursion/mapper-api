﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mapper_Api.Models
{
    public class CourseElement
    {
        [Required] [Key] public Guid CourseElementId { get; set; }
        
        public Guid? HoleId { get; set; }
        public Hole Hole { get; set; }
        
        [ForeignKey("CourseId")] public Guid CourseId { get; set; }
        public GolfCourse GolfCourse { get; set; }
    }
}