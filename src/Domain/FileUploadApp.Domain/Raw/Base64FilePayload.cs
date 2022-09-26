using System.ComponentModel.DataAnnotations;

namespace FileUploadApp.Domain.Raw;

public class Base64FilePayload
{
    public const string DataToken = "data";

    [Required]
    [MinLength(3)]
    public string Name { get; set; }

    [Required]
    [MinLength(3)]
    public string RawData { get; set; }
}
