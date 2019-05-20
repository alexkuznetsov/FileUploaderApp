namespace FileUploadApp.Storage
{
    public interface IFileStreamProvider<TKey, TFileStream>
    {
        TFileStream GetStreamAdapter(TKey id);
    }
}
