using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Core.Mvc;

public abstract class BaseApiController : ControllerBase
{
    private readonly IMediator mediator;

    protected BaseApiController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    protected async Task PublishAsync(INotification notification, CancellationToken cancellationToken = default)
    {
        await mediator.Publish(notification, cancellationToken);
    }

    protected async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        return await mediator.Send(request, cancellationToken);
    }
}
