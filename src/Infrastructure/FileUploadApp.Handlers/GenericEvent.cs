using MediatR;

namespace FileUploadApp.Features;

public abstract class GenericEvent : INotification
{
}

public abstract class ResultBase<TResult>
    where TResult : class, new()
{
    private static readonly TResult okVal = new();
    private static readonly TResult notFoundVal = new();

    public static TResult NotFound() => notFoundVal;
    public static TResult Ok() => okVal;

    public bool IsNotFound() => ReferenceEquals(this, notFoundVal);
}
