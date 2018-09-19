using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Mapper_Api.Models;
using Newtonsoft.Json;

namespace Mapper_Api.ViewModels
{
    public class PointViewModel : ElementViewModel
    {
        public int PointType { get; set; }
        public String Info { get; set; }
        public string GeoJson { get; set; }

        public static implicit operator PointViewModel(Point v) => new PointViewModel()
        {
            GeoJson = v.GeoJson,
            Info = v.Info,
            PointType = v.PointType,
        };
    }
}
