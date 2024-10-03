using System.Collections.Generic;

namespace FileUploadApp.Domain;

public class TokenPayload
{
    public required string Subject { get; set; }
    public string? Role { get; set; }
    public long Expires { get; set; }
    public required IEnumerable<TokenClaim> Claims { get; set; }
}