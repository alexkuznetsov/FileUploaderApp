using System;
using FileUploadApp.Interfaces;
using System.IO;
using System.Threading.Tasks;

namespace FileUploadApp.Storage.Filesystem
{
    public class SpecHandler
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

        public async Task<Spec> ReadSpecAsync(string file, System.Threading.CancellationToken cancellationToken = default)
        {
            var specFilePath = FormatSpecFilePath(file);

            if (File.Exists(specFilePath))
            {
                var contents = await File.ReadAllTextAsync(specFilePath, cancellationToken)
                    .ConfigureAwait(false);

                return await deserializer.DeserializeAsync<Spec>(contents)
                    .ConfigureAwait(false);
            }

            return default;
        }

        public async Task<Spec> WriteSpecAsync(string file, Spec spec, System.Threading.CancellationToken cancellationToken = default)
        {
            var specFilePath = FormatSpecFilePath(file);

            using (var writer = File.CreateText(specFilePath))
            {
                var contents = await serializer.SerializeAsync(spec)
                    .ConfigureAwait(false);

                await writer.WriteAsync(contents.AsMemory(), cancellationToken)
                    .ConfigureAwait(false);

                return spec;
            }
        }
    }
}
