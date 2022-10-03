using FileUploadApp.Domain;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.StreamAdapters
{
    public class CommonStreamStreamAdapter : StreamAdapter
    {
        private readonly Stream stream;

        public CommonStreamStreamAdapter(Stream stream)
        {
            this.stream = stream;
        }

        public override Stream Stream => stream;

        /// <summary>
        /// Копирует и удаляет оригинальный поток
        /// </summary>
        /// <param name="target"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
        {
            await Stream.CopyToAsync(target, cancellationToken).ConfigureAwait(false);
        }
    }
}
