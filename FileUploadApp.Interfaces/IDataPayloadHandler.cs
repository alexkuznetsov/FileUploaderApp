using FileUploadApp.Interfaces;
using System.Threading.Tasks;

namespace FileUploadApp.Interfaces
{
    public interface IDataPayloadHandler<TContext>
    {
        Task ApplyAsync(TContext context, IUploadService uploadsService);
    }

    public interface IFormJsonPayloadHandler<TContext> : IDataPayloadHandler<TContext>
    {

    }

    public interface IMultipathFormPayloadHandler<TContext> : IDataPayloadHandler<TContext>
    {

    }

    public interface IPlaintextPayloadHandler<TContext> : IDataPayloadHandler<TContext>
    {

    }
}
