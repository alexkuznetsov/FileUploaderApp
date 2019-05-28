namespace FileUploadApp.Domain
{
    public interface IHaveId<TId>
    {
        TId Id { get; }
    }
}