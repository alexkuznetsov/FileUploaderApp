﻿using FileUploadApp.Domain;
using System.IO;
using System.Threading;
using System.Threading.Tasks;


namespace FileUploadApp.StreamAdapters
{
    public class DownloadableStreamAdapter : StreamAdapter
    {
        private readonly string pathToFile;

        public DownloadableStreamAdapter(string pathToFile)
        {
            this.pathToFile = pathToFile;
        }

        public override Stream Stream => File.OpenRead(pathToFile);

        public override async Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
        {
            using (Stream)
            {
                await Stream.CopyToAsync(target, cancellationToken).ConfigureAwait(false);
            }
        }
    }
}