using System.ComponentModel.DataAnnotations;

namespace FileUploadApp.Domain.Raw;

public class Base64FilePayload
{
    public const string DataToken = "data";

    [Required]
    [MinLength(3)]
    public required string Name{ get; init; }

    [Required]
    [MinLength(3)]
    public required string RawData { get; init; }
}
