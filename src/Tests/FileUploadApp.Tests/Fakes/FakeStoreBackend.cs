using FileUploadApp.Domain;
using FileUploadApp.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Tests.Fakes
{
    internal class FakeStoreBackend : IStoreBackend<Guid, Metadata, Upload>, IFileStreamProvider<Guid, Stream>
    {
        private readonly Dictionary<Guid, Upload> keyValuePairs = new();

        public Task<Upload> FindAsync(Guid key, CancellationToken cancellationToken = default)
        {
            if (keyValuePairs.TryGetValue(key, out var value))
                return Task.FromResult(value);

            throw new NotImplementedException();
        }

        public Task DeleteAsync(Metadata key, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Stream GetStream(Guid id)
        {
            return keyValuePairs[id].Stream;
        }

        public Task SaveAsync(Upload file, CancellationToken cancellationToken = default)
        {
            keyValuePairs.Add(file.Id, file);

            return Task.FromResult(0);
        }
    }
}
