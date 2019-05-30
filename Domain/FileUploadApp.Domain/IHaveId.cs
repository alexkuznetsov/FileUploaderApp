namespace FileUploadApp.Domain
{
    public interface IHaveId<out TId>
    {
        TId Id { get; }
    }
}