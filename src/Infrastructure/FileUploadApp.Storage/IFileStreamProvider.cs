namespace FileUploadApp.Storage;

public interface IFileStreamProvider<in TKey, out TFileStream>
    where TKey : struct
    where TFileStream : class
{
    TFileStream GetStream(TKey id);
}