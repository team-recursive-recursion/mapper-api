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

        public List<GolfCourse> Courses { get; set; }
    }
}
