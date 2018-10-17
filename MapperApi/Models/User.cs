/***
 * Filename: ElementsController.cs
 * Author  : Eben du Toit
 * Class   : User
 *
 *      Entity model for User
 ***/
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mapper_Api.Models
{
    public class User
    {
        [Key] public Guid UserID { get; set; }
        [Required] public string Email { get; set; }
        [Required] public string Name { get; set; }
        [Required] public string Surname { get; set; }
        [Required] public string Password { get; set; }

        // Optional
        public string Token { get; internal set; }

        // Relations
        public List<Zone> Zones { get; set; }
    }
}
