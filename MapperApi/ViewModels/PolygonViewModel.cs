using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GeoJSON.Net.Contrib.Wkb;
using Mapper_Api.Models;
using Newtonsoft.Json;

namespace Mapper_Api.ViewModels
{
    public class PolygonViewModel : ElementViewModel
    {
        public int PolygonType { get; set; }
        public byte[] PolygonRaw { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public string GeoJson { get; set; }

        public static implicit operator PolygonViewModel(Polygon v) => new PolygonViewModel()
        {
            GeoJson = v.GeoJson,
            PolygonType = v.PolygonType,
        };
    }
}
