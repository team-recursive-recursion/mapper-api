using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Mapper_Api.Models;

namespace Mapper_Api.ViewModels
{

    public class ElementViewModel
    {
        public Guid ElementId { get; set; }
        public Element.ElementTypes ElementType { get; set; }
        public Guid? HoleId { get; set; }
        public Guid CourseId { get; set; }
        public static implicit operator ElementViewModel(Element v) => new PointViewModel()
        {
            HoleId = v.HoleId,
            CourseId = v.CourseId,
            ElementId = v.ElementId,
            ElementType = v.ElementType
        };

    }

}