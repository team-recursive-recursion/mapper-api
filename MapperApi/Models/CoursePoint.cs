using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GeoJSON.Net.Contrib.Wkb;

namespace Mapper_Api.Models
{
    public class Point : CourseElement
    {
        public enum PointTypes
        {
            PIN = 0,
            HOLE = 1,
        }

        [Required] public PointTypes Type { get; set; }
        [Required] public byte[] PointRaw { get; set; }
        public DateTime CreatedAt;
        public DateTime UpdatedAt;

        [NotMapped]
        public GeoJSON.Net.Geometry.Point JsonPoint
        {
            get => PointRaw.ToGeoJSONObject<GeoJSON.Net.Geometry.Point>();
        }
    }
}