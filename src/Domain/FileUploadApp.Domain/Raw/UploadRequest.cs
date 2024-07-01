namespace FileUploadApp.Domain.Raw;

public record UploadRequest(Base64FilePayload[]? Files = default,
    string[]? Links = default);
