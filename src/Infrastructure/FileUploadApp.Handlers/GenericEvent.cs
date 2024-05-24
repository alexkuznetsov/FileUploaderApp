using MediatR;

namespace FileUploadApp.Features;

public abstract class GenericEvent : INotification
{
}

public abstract class ResultBase<TResult>
    where TResult : class, new()
{
    private static readonly TResult OkVal = new();
    private static readonly TResult NotFoundVal = new();

    public static TResult NotFound() => NotFoundVal;
    public static TResult Ok() => OkVal;

    public bool IsNotFound() => ReferenceEquals(this, NotFoundVal);
}
