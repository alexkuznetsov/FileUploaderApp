using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using FileUploadApp.Domain;
using FileUploadApp.Imaging;
using FileUploadApp.Interfaces;

using MediatR;

using Microsoft.Extensions.Logging;

namespace FileUploadApp.Features.Commands;

public class UploadFiles
{
    public class Event : GenericEvent
    {
        public Event(IEnumerable<Upload> uploadedFiles)
        {
            UploadedFiles = uploadedFiles;
        }

        public IEnumerable<Upload> UploadedFiles { get; }
    }

    public class Handler : INotificationHandler<Event>
    {
        private readonly IStore<Guid, Upload, UploadResultRow> _store;
        private readonly AppConfiguration _appConfiguration;
        private readonly ILogger<Handler> _logger;

        public Handler(IStore<Guid, Upload, UploadResultRow> store
            , AppConfiguration appConfiguration
            , ILogger<Handler> logger)
        {
            this._store = store;
            this._appConfiguration = appConfiguration;
            this._logger = logger;
        }

        public async Task Handle(Event notification, CancellationToken cancellationToken)
        {
            var tasks = notification.UploadedFiles.Select(SaveFileAsync).ToArray();

            await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        private async Task<UploadResultRow> SaveFileAsync(Upload uploadModel)
        {
            _logger.LogInformation("Saving file {fileName}. Content type: {fileContentType}"
                , uploadModel.Name, uploadModel.ContentType);

            var result = await _store.StoreAsync(uploadModel).ConfigureAwait(false);

            if (!uploadModel.IsImage())
            {
                uploadModel.Stream.Dispose();

                return result;
            }

            _logger.LogInformation("Saving preview for file {fileName}. Content type: {fileContentType}",
                uploadModel.Name, uploadModel.ContentType);

            using var previewData = ImageHelper.Resize(_appConfiguration.PreviewSize
                    , uploadModel.Stream
                    , _appConfiguration.PreviewContentType);

            var preview = new Upload(
                  id: uploadModel.PreviewId
                , previewId: Guid.Empty
                , num: uploadModel.Number
                , name: $"{Upload.PreviewPrefix}{uploadModel.Name}"
                , contentType: _appConfiguration.PreviewContentType
                , data: previewData);

            result.Preview = await _store.StoreAsync(preview).ConfigureAwait(false);

            return result;
        }
    }

}
