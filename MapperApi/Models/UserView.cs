using System.ComponentModel.DataAnnotations;

namespace Mapper_Api.Models
{
    public class UserView
    {
        [Required] public string Email { get; set; }
        [Required] public string Password { get; set; }
    }
}