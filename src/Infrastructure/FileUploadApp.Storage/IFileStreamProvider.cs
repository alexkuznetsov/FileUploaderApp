namespace FileUploadApp.Storage;

public interface IFileStreamProvider<in TKey, out TFileStream>
    where TKey : struct
    where TFileStream : class
{
    TFileStream GetStreamAdapter(TKey id);
}