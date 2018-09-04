using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Mapper_Api.Models;

namespace Mapper_Api.ViewModels
{

    public class HoleViewModel
    {
        public Guid HoleId { get; set; }
        public string Name { get; set; }
        public Guid CourseId { get; set; }
        public List<ElementViewModel> Elements { get; set; }

    }
}