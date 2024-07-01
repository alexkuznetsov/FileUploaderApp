using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using FileUploadApp.Domain;
using FileUploadApp.Storage;

namespace FileUploadApp.Tests.Fakes;

internal class FakeStoreBackend : IStoreBackend<Guid, Metadata, Upload>, IFileStreamProvider<Guid, Stream>
{
    private readonly Dictionary<Guid, Upload> _keyValuePairs = [];

    public ValueTask<Upload?> FindAsync(Guid key, CancellationToken cancellationToken = default)
    {
        _keyValuePairs.TryGetValue(key, out var value);
        return ValueTask.FromResult(value);
    }

    public Task DeleteAsync(Metadata key, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Stream GetStream(Guid id)
    {
        return _keyValuePairs[id].Stream;
    }

    public Task SaveAsync(Upload file, CancellationToken cancellationToken = default)
    {
        _keyValuePairs.Add(file.Id, file);

        return Task.FromResult(0);
    }
}
