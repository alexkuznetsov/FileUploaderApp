using System.ComponentModel.DataAnnotations;

namespace FileUploadApp.Domain.Raw;

public record Base64FilePayload(
    [property: Required][property: MinLength(3)] string Name,
    [property: Required][property: MinLength(3)] string RawData)
{
    public const string DataToken = "data";
}
