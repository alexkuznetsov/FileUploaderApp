using FileUploadApp.Domain.Dirty;
using FileUploadApp.Events;
using System.Collections.Generic;
using System.Linq;

namespace FileUploadApp.Core.Infrastructure
{
    internal class UploadRequestEventBuilder
    {
        delegate GenericEvent GenericEventBuilder<TSource>(IEnumerable<TSource> sources);

        private readonly UploadRequest uploadRequest;

        public UploadRequestEventBuilder(UploadRequest uploadRequest)
        {
            this.uploadRequest = uploadRequest;
        }

        public IEnumerable<GenericEvent> BuildEvents() => FileEvents().Concat(LinkEvents());

        private IEnumerable<GenericEvent> FileEvents() => ConvertIfAny(uploadRequest?.Files?.AsFileDesciptors(), (f) => new ProcessFileDescriptorEvent(f.ToArray()));
        private IEnumerable<GenericEvent> LinkEvents() => ConvertIfAny(uploadRequest?.Links, (f) => new ProcessImageUriEvent(f.ToArray()));

        private static IEnumerable<GenericEvent> ConvertIfAny<TRequest>(IEnumerable<TRequest> source, GenericEventBuilder<TRequest> builder)
        {
            var isFilesNotEmpty = source != null && source?.Count() > 0;

            if (isFilesNotEmpty)
            {
                yield return builder(source);
            }
        }
    }
}
