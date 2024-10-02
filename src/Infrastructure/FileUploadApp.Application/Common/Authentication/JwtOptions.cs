namespace FileUploadApp.Application.Common.Authentication;

public class JwtOptions
{
    public const string SectionName = "jwt";
    public string SecretKey { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public int ExpiryMinutes { get; set; }
    public bool ValidateLifetime { get; set; }
    public bool ValidateAudience { get; set; }
    public string? ValidAudience { get; set; }
}