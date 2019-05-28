using System.ComponentModel.DataAnnotations;

namespace FileUploadApp.Domain.Authtentication
{
    public class AuthenticationRequest
    {
        [Required]
        [MinLength(3)]
        public string Username { get; set; }

        [Required]
        [MinLength(3)]
        public string Password { get; set; }
    }
}
