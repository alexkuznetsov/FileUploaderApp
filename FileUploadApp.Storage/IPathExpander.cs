namespace FileUploadApp.Storage
{
    public interface IPathExpander<TKey>
    {
        string BuildPathAndCheckDir(TKey id, bool createIfNotExists);
    }
}
