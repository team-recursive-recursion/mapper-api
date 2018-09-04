
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class LiveUser {
    [Required][Key]
    public Guid UserID { get; set; }
    public List<LiveLocation> Locations { get; set; }
}