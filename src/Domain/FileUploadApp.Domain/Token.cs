using System.Collections.Generic;

namespace FileUploadApp.Domain;

public class Token : IHaveId<string>
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
    public long Expires { get; set; }
    public required string Id { get; set; }
    public string? Role { get; set; }
    public required IEnumerable<TokenClaim> Claims { get; set; }
}

public record TokenClaim(string Key, string Value);