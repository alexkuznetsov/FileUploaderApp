using FileUploadApp.Domain;
using FileUploadApp.Interfaces;
using FileUploadApp.StreamWrappers;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FileUploadApp.Storage.Filesystem
{
    public class FileSystemStore : IStorage
    {
        private readonly string basePath;
        private readonly SpecHandler specHandler;

        public FileSystemStore(string basePath, SpecHandler specHandler)
        {
            if (string.IsNullOrWhiteSpace(basePath))
            {
                throw new ArgumentException("basePath should be a valid absolute path", nameof(basePath));
            }

            this.basePath = basePath;
            this.specHandler = specHandler ?? throw new ArgumentNullException(nameof(specHandler));
        }

        public async Task<UploadedFile> ReceiveAsync(string fileId)
        {
            var filePath = BuildPathAndCheckDir(fileId);
            var spec = await specHandler.ReadSpecAsync(filePath).ConfigureAwait(false);

            if (spec == null)
            {
                throw new ArgumentNullException(nameof(spec), "File saved with errors");
            }

            return new UploadedFile
            (
                contentType: spec.ContentType,
                name: spec.Name,
                width: spec.Width,
                height: spec.Height,
                streamWrapper: new DownloadableStreamWrapper(filePath)
            );
        }

        public async Task<UploadResultRow> StoreAsync(UploadedFile file)
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
            )).ConfigureAwait(false);

            using (var wri = File.OpenWrite(filePath))
            {
                await file.Stream.CopyToAsync(wri).ConfigureAwait(false);
                await wri.FlushAsync().ConfigureAwait(false);
            }

            return new UploadResultRow
            {
                ContentType = spec.ContentType,
                Height = spec.Height,
                Width = spec.Width,
                Id = spec.Name,
                Path = filePath
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
