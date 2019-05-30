using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace FileUploadApp.Domain.Authentication
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
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
