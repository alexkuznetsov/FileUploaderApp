using FileUploadApp.Interfaces;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Storage.Filesystem
{
    internal class SpecHandler
    {
        private static readonly string SpecFileExtension = ".spec";

        private readonly ISerializer serializer;
        private readonly IDeserializer deserializer;

        public SpecHandler(ISerializer serializer, IDeserializer deserializer)
        {
            this.serializer = serializer;
            this.deserializer = deserializer;
        }

        private static string FormatSpecFilePath(string file) => file + SpecFileExtension;

        public async Task<Spec> ReadSpecAsync(string file, CancellationToken cancellationToken = default)
        {
            var specFilePath = FormatSpecFilePath(file);

            if (File.Exists(specFilePath))
            {
                var contents = await File.ReadAllTextAsync(specFilePath, cancellationToken)
                    .ConfigureAwait(false);

                return deserializer.Deserialize<Spec>(contents);
            }

            return default;
        }

        public async Task<Spec> WriteSpecAsync(string path, Domain.Upload file, CancellationToken cancellationToken = default)
        {
            var spec = new Spec
            (
                id: file.Id,
                name: file.Name,
                contentType: file.ContentType,
                dateTime: DateTime.UtcNow);

            var specFilePath = FormatSpecFilePath(path);

            using (var writer = File.CreateText(specFilePath))
            {
                var contents = serializer.Serialize(spec);

                await writer.WriteAsync(contents.AsMemory(), cancellationToken)
                    .ConfigureAwait(false);

                return spec;
            }
        }
    }
}
