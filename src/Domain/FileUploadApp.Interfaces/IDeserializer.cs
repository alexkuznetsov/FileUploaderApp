namespace FileUploadApp.Interfaces
{
    public interface IDeserializer
    {
        TObject Deserialize<TObject>(string payload);
    }
}
