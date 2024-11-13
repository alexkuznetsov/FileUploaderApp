using MediatR;

namespace FileUploadApp.Application.Common.Messaging;

public abstract record GenericEvent : INotification;

public interface IMessage<out TResponse> : IRequest<TResponse>;
public interface ICommand<out TResponse> : IRequest<TResponse>;

public interface IMessageHandler<in TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : IMessage<TResponse>;

public interface ICommandHandler<in TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : ICommand<TResponse>;

#pragma warning disable CA1711 // Identifiers should not have incorrect suffix
public interface IEventHandler<in TRequest> : INotificationHandler<TRequest>
#pragma warning restore CA1711 // Identifiers should not have incorrect suffix
    where TRequest : GenericEvent;