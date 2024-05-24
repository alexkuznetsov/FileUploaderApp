namespace FileUploadApp.Domain.Raw;

public record UploadRequest(Base64FilePayload[] Files, string[] Links);
