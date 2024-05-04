using System.ComponentModel.DataAnnotations;

namespace DatingApp.DTOs
{
    public class LoginDTo
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public string Token { get; set; }
        public string PhotoUrl { get; set; }


    }
}
