namespace FileUploadApp.Storage
{
    public interface IFileStreamProvider<in TKey, out TFileStream>
    {
        TFileStream GetStreamAdapter(TKey id);
    }
}
