using FileUploadApp.Interfaces;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Storage.Filesystem
{
    internal class MetadataFSStoreBackend : IStoreBackend<Guid, Metadata>
    {
        private static readonly string SpecFileExtension = ".spec";

        private readonly IPathExpander<Guid> pathExpander;
        private readonly ISerializer serializer;
        private readonly IDeserializer deserializer;

        public MetadataFSStoreBackend(IPathExpander<Guid> pathExpander, ISerializer serializer, IDeserializer deserializer)
        {
            this.pathExpander = pathExpander;
            this.serializer = serializer;
            this.deserializer = deserializer;
        }

        public async Task SaveAsync(Metadata file, CancellationToken cancellationToken = default)
        {
            var path = pathExpander.BuildPathAndCheckDir(file.Id, true);
            var specFilePath = FormatSpecFilePath(path);

            using (var writer = File.CreateText(specFilePath))
            {
                var contents = serializer.Serialize(file);

                await writer.WriteAsync(contents.AsMemory(), cancellationToken)
                    .ConfigureAwait(false);
            }
        }

        private static string FormatSpecFilePath(string file) => file + SpecFileExtension;

        public async Task<Metadata> FindAsync(Guid key, CancellationToken cancellationToken = default)
        {
            var path = pathExpander.BuildPathAndCheckDir(key, false);
            var specFilePath = FormatSpecFilePath(path);

            if (File.Exists(specFilePath))
            {
                var contents = await File.ReadAllTextAsync(specFilePath, cancellationToken)
                    .ConfigureAwait(false);

                return deserializer.Deserialize<Metadata>(contents);
            }

            return default;
        }
    }
}
