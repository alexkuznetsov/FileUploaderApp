using FileUploadApp.Domain;
using FileUploadApp.Interfaces;
using FileUploadApp.StreamAdapters;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Storage.Filesystem
{
    public class FileSystemStore : IStorage<Upload, UploadResultRow>
    {
        private readonly string basePath;
        private readonly SpecHandler specHandler;

        internal FileSystemStore(string basePath
            , ISerializer serializer
            , IDeserializer deserializer)
        {
            if (string.IsNullOrWhiteSpace(basePath) || !Directory.Exists(basePath))
            {
                throw new ArgumentException("basePath should be a valid  path", nameof(basePath));
            }

            this.basePath = basePath;
            specHandler = new SpecHandler(serializer, deserializer);
        }

        public async Task<Upload> ReceiveAsync(string fileId, CancellationToken cancellationToken = default)
        {
            var filePath = BuildPathAndCheckDir(fileId);
            var spec = await specHandler.ReadSpecAsync(filePath, cancellationToken).ConfigureAwait(false);

            if (spec == null)
            {
                throw new ArgumentNullException(nameof(spec), "File saved with errors");
            }

            return new Upload
            (
                num: 0,
                contentType: spec.ContentType,
                name: spec.Name,
                width: spec.Width,
                height: spec.Height,
                streamAdapter: new DownloadableStreamAdapter(filePath)
            );
        }

        public async Task<UploadResultRow> StoreAsync(Upload file, CancellationToken cancellationToken = default)
        {
            var fileId = Guid.NewGuid().ToString();
            var filePath = BuildPathAndCheckDir(fileId);

            var spec = await specHandler.WriteSpecAsync(filePath, new Spec
            (
                name: file.Name,
                cntentType: file.ContentType,
                path: fileId,
                height: file.Height,
                width: file.Width,
                dateTime: DateTime.UtcNow
            ), cancellationToken).ConfigureAwait(false);

            using (var wri = File.OpenWrite(filePath))
            {
                await file.Stream.CopyToAsync(wri, cancellationToken).ConfigureAwait(false);
                await wri.FlushAsync(cancellationToken).ConfigureAwait(false);
            }

            return new UploadResultRow
            {
                Id = spec.Path,
                Name = file.Name,
                ContentType = spec.ContentType,
                Number = file.Number,
                Height = spec.Height,
                Width = spec.Width,
            };
        }

        private string BuildPathAndCheckDir(string fileId)
        {
            if (string.IsNullOrWhiteSpace(fileId))
            {
                throw new ArgumentException("fileId can not be null or empty string", nameof(fileId));
            }

            var span = fileId.ToCharArray();

            var foldersPath = Path.Combine(basePath, new string(span.Slice(0, 2)),
                new string(span.Slice(2, 2)),
                new string(span.Slice(4, 2)));

            if (!Directory.Exists(foldersPath))
            {
                Directory.CreateDirectory(foldersPath);
            }

            return Path.Combine(foldersPath, fileId);
        }
    }
}
