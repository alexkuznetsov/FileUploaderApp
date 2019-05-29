using FileUploadApp.Storage;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Tests.Fakes
{
    internal class FakeMetadataStoreBackend : IStoreBackend<Guid, Metadata>
    {
        private readonly Dictionary<Guid, Metadata> keyValuePairs = new Dictionary<Guid, Metadata>();

        public Task<Metadata> FindAsync(Guid key, CancellationToken cancellationToken = default)
        {
            if (keyValuePairs.TryGetValue(key, out var value))
                return Task.FromResult(value);

            throw new NotImplementedException();
        }

        public Task SaveAsync(Metadata file, CancellationToken cancellationToken = default)
        {
            keyValuePairs.Add(file.Id, file);

            return Task.FromResult(0);
        }
    }
}
