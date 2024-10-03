using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using FileUploadApp.Application.Common.Messaging;
using FileUploadApp.Domain;
using FileUploadApp.Interfaces;

namespace FileUploadApp.Application.Uploading.Queries;

public class GetUploadedResults
{
    public record Query(IEnumerable<Upload> Uploads) : IMessage<UploadResult>;

    public sealed class Handler(IStore<Guid, Upload, UploadResultRow> store)
        : IMessageHandler<Query, UploadResult>
    {
        public async Task<UploadResult> Handle(Query request, CancellationToken cancellationToken)
        {
            var ids = request.Uploads
                .Select(x => new Tuple<Guid, Guid>(x.Id, x.PreviewId.GetValueOrDefault()))
                .ToArray();
            var tasks = ids.Select(x => ReceiveAsync(x, cancellationToken)).ToArray();
            var results = await Task.WhenAll(tasks) ?? [];

            return new UploadResult(results);
        }

        private async Task<UploadResultRow?> ReceiveAsync(Tuple<Guid, Guid> fileIdPreviewId, CancellationToken cancellationToken)
        {
            var (fileId, previewId) = fileIdPreviewId;
            var file = await store.ReceiveAsync(fileId, cancellationToken).ConfigureAwait(false);

            if (file == null)
                return null;

            var row = new UploadResultRow(file.Id, file.Number, file.Name, file.ContentType);

            if (!file.IsImage()) return row;

            var preview = await store.ReceiveAsync(previewId, cancellationToken).ConfigureAwait(false);

            if (preview != null)
                row.Preview = new FileEntity(preview.Id, preview.Number, preview.Name, preview.ContentType);

            return row;
        }
    }

}
