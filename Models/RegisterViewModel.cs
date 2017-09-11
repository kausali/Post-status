using System.ComponentModel.DataAnnotations;
using wall.Models;

namespace wall.Models
{
    public class RegisterViewModel : BaseEntity
    {        
        [Required(ErrorMessage="First name cannot be less than 2 characters!")]
        [MinLength(2)]
        [RegularExpression(@"^[a-zA-Z]+$")]
        public string first_name { get; set; }

        [Required(ErrorMessage="Last name cannot be less than 2 characters!")]
        [MinLength(2)]
        [RegularExpression(@"^[a-zA-Z]+$")]
        public string last_name { get; set; }
 
        [Required]
        [EmailAddress]
        public string email { get; set; }
 
        [Required(ErrorMessage="Password should be at least 8 characters long!")]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string password { get; set; }
 
        [Compare("password", ErrorMessage = "Password and confirmation password must match.")]
        public string config_password { get; set; }
    }
}