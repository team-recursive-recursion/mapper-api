using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GeoJSON.Net.Contrib.Wkb;
using Newtonsoft.Json;

namespace Mapper_Api.Models
{
    public class Element
    {
        public enum ElementTypes
        {
            POLYGON = 0,
            POINT = 1
        }

        [Required] [Key] 
        public Guid? ElementId { get; set; }

        [Required] 
        public Guid? ZoneID { get; set; }

        [Required] 
        public Zone Zone { get; set; }

        [Required] 
        public ElementTypes? ElementType { get; set; }

        [Required] 
        public byte[] Raw {get; set;}

        [Required]
        public int? ClassType {get; set;}

        [NotMapped]
        public virtual string GeoJson {get; set;}
        // Optional
        public String Info { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class Polygon : Element
    {
        public override string GeoJson
        {
            get =>
                    JsonConvert.SerializeObject(
                            Raw.ToGeoJSONObject<GeoJSON.Net.Geometry.Polygon>());
            set =>
                    Raw = JsonConvert
                    .DeserializeObject<GeoJSON.Net.Geometry.Polygon>(value).ToWkb();
        }
    }

    public class Point : Element
    {
        public override string GeoJson
        {
            get =>
                    JsonConvert.SerializeObject(Raw
                            .ToGeoJSONObject<GeoJSON.Net.Geometry.Point>());
            set =>
                    Raw = JsonConvert
                            .DeserializeObject<GeoJSON.Net.Geometry.Point>(
                                    value).ToWkb();
        }
    }
}
