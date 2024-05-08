using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using WebAPIDatingAPP.DATA;

namespace DatingApp.DTOs
{
    public class RegisterDto
    {
        public string Gender { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string KnownAs { get; set; }
        [Required]
        [JsonConverter(typeof(DateOnlyConverter))]
        public DateOnly? DateOfBirth { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.")]
        public string Password { get; set; }
        [Required]
        [StringLength(8, MinimumLength = 4)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }



    }



    
}