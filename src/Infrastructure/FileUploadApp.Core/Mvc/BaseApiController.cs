using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace FileUploadApp.Core.Mvc;

public abstract class BaseApiController : ControllerBase
{
    private readonly IMediator _mediator;

    protected BaseApiController(IMediator mediator)
    {
        this._mediator = mediator;
    }

    protected async Task PublishAsync(INotification notification, CancellationToken cancellationToken = default)
    {
        await _mediator.Publish(notification, cancellationToken);
    }

    protected async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(request, cancellationToken);
    }
}
