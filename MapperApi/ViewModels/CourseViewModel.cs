using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Mapper_Api.Models;

namespace Mapper_Api.ViewModels
{
    public class CourseViewModel
    {
        public Guid CourseId { get; set; }
        public string CourseName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Guid UserId { get; set; }

        public List<Hole> Holes { get; set; }
        public List<ElementViewModel> Elements { get; set; }
    }
}
