using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using FileUploadApp.Storage;

namespace FileUploadApp.Tests.Fakes;

internal class FakeMetadataStoreBackend : IStoreBackend<Guid, Metadata, Metadata>
{
    private readonly Dictionary<Guid, Metadata> _keyValuePairs = [];

    public ValueTask<Metadata?> FindAsync(Guid key, CancellationToken cancellationToken = default)
    {
        _keyValuePairs.TryGetValue(key, out var value);

        return ValueTask.FromResult(value);
    }

    public Task DeleteAsync(Metadata key, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task SaveAsync(Metadata file, CancellationToken cancellationToken = default)
    {
        _keyValuePairs.Add(file.Id, file);

        return Task.FromResult(0);
    }
}
