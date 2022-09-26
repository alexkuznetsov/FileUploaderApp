using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FileUploadApp.Domain;

public class TokenPayload
{
    public string Subject { get; set; }
    public string Role { get; set; }
    public long Expires { get; set; }
    public IDictionary<string, string> Claims { get; set; }
}