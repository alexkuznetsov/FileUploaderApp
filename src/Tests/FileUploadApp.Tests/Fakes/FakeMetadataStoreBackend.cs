using System;
using System.Threading;
using System.Threading.Tasks;

using FileUploadApp.Storage;

namespace FileUploadApp.Tests.Fakes;

internal class FakeMetadataStoreBackend : TestData, IStoreBackend<Guid, Metadata, Metadata>
{
    public ValueTask<Metadata?> FindAsync(Guid key, CancellationToken cancellationToken = default)
    {
        return ValueTask.FromResult<Metadata?>(DefaultMetadata);
    }

    public Task DeleteAsync(Metadata key, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task SaveAsync(Metadata file, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(0);
    }
}
