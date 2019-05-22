using FileUploadApp.Core;
using System;
using System.IO;

namespace FileUploadApp.Storage.Filesystem
{
    public abstract class FileStoreBackendBase
    {
        protected FileStoreBackendBase(StorageConfiguration storageConfiguration)
        {
            StorageConfiguration = storageConfiguration;
        }

        protected StorageConfiguration StorageConfiguration { get; }

        protected string BuildPathAndCheckDir(Guid fileId, bool createIfNotExists)
        {
            if (Guid.Empty.Equals(fileId))
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

            return Path.Combine(foldersPath, fileIdStr);
        }
    }
}
