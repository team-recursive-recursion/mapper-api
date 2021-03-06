

/***
 * Filename: ElementsController.cs
 * Author  : Eben du Toit
 * Class   : PlayerLocation
 *
 *      Entity model for PlayerLocation
 ***/
using System;
using Mapper_Api.Models;

public class PlayerLocation
{

    public DateTime CreatedAt { get; set; }
    public Guid UserID { get; set; }

    public String GeoJSON { get; set; }
    
        public static implicit operator PlayerLocation(LiveLocation v) => new PlayerLocation()
        {
            UserID = v.UserID,
            CreatedAt = v.CreatedAt,
        };
}