namespace FileUploadApp.Domain;

public class UploadResult
{
    public UploadResult(UploadResultRow[] result)
    {
        Result = result;
    }

    public UploadResultRow[] Result { get; }
}
