
/***
 * Filename: ElementsController.cs
 * Author  : Eben du Toit
 * Class   : LiveUser,LiveLocation 
 *
 *      Entity model for Live location.
 ***/

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GeoJSON.Net.Contrib.Wkb;
using Newtonsoft.Json;
namespace Mapper_Api.Models
{

    public class LiveUser
    {

        [Required] [Key] public Guid UserID { get; set; }
        public List<LiveLocation> Locations { get; set; }

    }

    public class LiveLocation
    {
        public DateTime CreatedAt { get; set; }
        public Guid UserID { get; set; }

        public byte[] PointRaw { get; set; }

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