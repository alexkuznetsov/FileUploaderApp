using System;
using System.IO;
using FileUploadApp.Core;
using Microsoft.Extensions.Logging;

namespace FileUploadApp.Storage.Filesystem
{
    public abstract class FileStoreBackendBase
    {
        private readonly ILogger<FileStoreBackendBase> logger;

        protected FileStoreBackendBase(StorageConfiguration storageConfiguration, ILogger<FileStoreBackendBase> logger)
        {
            this.logger = logger;
            StorageConfiguration = storageConfiguration;
        }

        private StorageConfiguration StorageConfiguration { get; }

        protected string BuildPathAndCheckDir(Guid fileId, bool createIfNotExists)
        {
            if (fileId == null || fileId.Equals(Guid.Empty))
            {
                throw new ArgumentException("fileId can not be null or empty", nameof(fileId));
            }

            var fileIdStr = fileId.ToString();
            var span = fileIdStr.ToCharArray();

            var foldersPath = Path.Combine(StorageConfiguration.BasePath,
                new string(span.Slice(0, 2)),
                new string(span.Slice(2, 2)),
                new string(span.Slice(4, 2)));

            if (createIfNotExists && !Directory.Exists(foldersPath))
            {
                Directory.CreateDirectory(foldersPath);
            }

            logger.LogInformation("Expanded path for {0} is {1}",
                fileId.ToString()
                , Path.GetFullPath(Path.Combine(foldersPath, fileIdStr)));

            return Path.Combine(foldersPath, fileIdStr);
        }

        private const int MaxDepth = 3;

        protected static void RemoveDirIfEmpty(string directoryPath, int depth = 0)
        {
            while (true)
            {
                if (depth == MaxDepth) return;
                if (!Dir.IsDirectoryEmpty(directoryPath)) return;

                Dir.Delete(directoryPath);
                directoryPath = Path.GetDirectoryName(directoryPath);
                depth++;
            }
        }
    }
}