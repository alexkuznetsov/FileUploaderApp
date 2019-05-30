﻿using FileUploadApp.Interfaces;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Storage.Filesystem
{
    public class MetadataFsStoreBackend : FileStoreBackendBase, IStoreBackend<Guid, Metadata>
    {
        private const string SpecFileExtension = ".spec";

        private readonly ISerializer serializer;
        private readonly IDeserializer deserializer;

        public MetadataFsStoreBackend(StorageConfiguration storageConfiguration
            , ISerializer serializer
            , IDeserializer deserializer)
            : base(storageConfiguration)
        {
            this.serializer = serializer;
            this.deserializer = deserializer;
        }

        public async Task SaveAsync(Metadata file, CancellationToken cancellationToken = default)
        {
            var path = BuildPathAndCheckDir(file.Id, true);
            var specFilePath = FormatSpecFilePath(path);

            using (var writer = File.CreateText(specFilePath))
            {
                var contents = serializer.Serialize(file);

                await writer.WriteAsync(contents.AsMemory(), cancellationToken)
                    .ConfigureAwait(false);
            }
        }

        public async Task<Metadata> FindAsync(Guid key, CancellationToken cancellationToken = default)
        {
            var path = BuildPathAndCheckDir(key, false);
            var specFilePath = FormatSpecFilePath(path);

            if (!File.Exists(specFilePath)) return default;
            
            var contents = await File.ReadAllTextAsync(specFilePath, cancellationToken)
                .ConfigureAwait(false);

            return deserializer.Deserialize<Metadata>(contents);

        }

        private static string FormatSpecFilePath(string file) => file + SpecFileExtension;
    }
}