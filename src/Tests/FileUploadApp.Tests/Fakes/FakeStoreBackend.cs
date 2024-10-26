using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using FileUploadApp.Domain;
using FileUploadApp.Interfaces;
using FileUploadApp.Storage;

namespace FileUploadApp.Tests.Fakes;

internal sealed class FakeFileSystemStore : TestData, IStore<Guid, Upload, UploadResultRow>
{
    public Task<bool> DeleteAsync(Guid fileId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(false);
    }

    public Task<Upload?> ReceiveAsync(Guid fileId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<Upload?>(FakeUpload);
    }

    private static readonly UploadResultRow FakeUploadResultRow =
        new UploadResultRow(FakeUpload.Id, FakeUpload.Number, FakeUpload.Name, FakeUpload.ContentType);

    public Task<UploadResultRow> StoreAsync(Upload file, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(FakeUploadResultRow);
    }
}

internal class FakeStoreBackend : TestData, IStoreBackend<Guid, Metadata, Upload>, IFileStreamProvider<Guid, Stream>
{
    public ValueTask<Upload?> FindAsync(Guid key, CancellationToken cancellationToken = default)
    {
        return ValueTask.FromResult<Upload?>(FakeUpload);
    }

    public Task DeleteAsync(Metadata key, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Stream GetStream(Guid id)
    {
        return FakeDownloadUriResponse.Stream;
    }

    public Task SaveAsync(Upload file, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
