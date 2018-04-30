﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GeoJSON.Net.Contrib.Wkb;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;

namespace Mapper_Api.Models
{
    public class CoursePolygon : CourseElement
    {
        public enum PolygonTypes
        {
            ROUGH = 0,
            GREEN = 1,
            FAIRWAY = 2,
            BUNKER = 3,
            WATERHAZARD = 4
        }

        [Required] public PolygonTypes Type { get; set; }
        [Required] public byte[] PolygonRaw { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [NotMapped]
        public string GeoJson
        {
            get => JsonConvert.SerializeObject(PolygonRaw.ToGeoJSONObject<Polygon>());
            set => PolygonRaw = JsonConvert.DeserializeObject<Polygon>(value).ToWkb();
        }
    }
}