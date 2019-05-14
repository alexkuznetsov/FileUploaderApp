namespace FileUploadApp.Interfaces
{
    public interface IStorageProvider<TIn, TOut>
    {
        IStorage<TIn, TOut> GetStorage();
    }
}
