using FileUploadApp.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Handlers
{
    public class PlainTextRequestEventHandler : INotificationHandler<PlainTextRequestEvent>
    {
        public Task Handle(PlainTextRequestEvent notification, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
