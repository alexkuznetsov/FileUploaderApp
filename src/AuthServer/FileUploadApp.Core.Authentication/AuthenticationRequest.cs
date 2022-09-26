using System.ComponentModel.DataAnnotations;

namespace FileUploadApp.Authentication;

public class AuthenticationRequest
{
    [Required]
    [MinLength(3)]
    public string Username { get; set; }

    [Required]
    [MinLength(3)]
    public string Password { get; set; }
}
