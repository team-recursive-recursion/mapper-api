using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GeoJSON.Net.Contrib.Wkb;
using Newtonsoft.Json;

namespace Mapper_Api.Models
{
    public class Point : CourseElement
    {
        public enum PointTypes
        {
            PIN = 0,
            HOLE = 1,
            TEE = 2
        }

        public DateTime CreatedAt;
        public DateTime UpdatedAt;

        [Required] public PointTypes Type { get; set; }
        [Required] public byte[] PointRaw { get; set; }

        [NotMapped]
        public string GeoJson
        {
            get =>
                    JsonConvert.SerializeObject(PointRaw
                            .ToGeoJSONObject<GeoJSON.Net.Geometry.Point>());
            set =>
                    PointRaw = JsonConvert
                            .DeserializeObject<GeoJSON.Net.Geometry.Point>(
                                    value).ToWkb();
        }
    }
}
