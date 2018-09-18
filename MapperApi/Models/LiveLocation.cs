
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GeoJSON.Net.Contrib.Wkb;
using Newtonsoft.Json;

public class LiveLocation
{
    public DateTime CreatedAt { get; set; }

    public Guid? UserID { get; set; }

    [Required]
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