namespace FileUploadApp.Domain.Raw;

public class UploadRequest
{
    public Base64FilePayload[] Files { get; set; }

    public string[] Links { get; set; }
}