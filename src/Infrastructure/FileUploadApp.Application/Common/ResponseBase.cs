using System;

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FileUploadApp.Application.Common;

public record class ResponseBase
{
    public int State { get; init; }
    public Error[] Errors { get; init; } = [];

    public ResponseBase() : this(ResultState.Ok) { }

    public ResponseBase(int state) => State = state;
    public ResponseBase(ResultState state) => State = (int)state;

}


public abstract record ResultBase<TBase> : ResponseBase
    where TBase : ResultBase<TBase>, new()
{
    private static readonly Func<TBase> FactoryNotFound = () => new() { State = (int)ResultState.NotFound };
    private static readonly Func<TBase> FactoryOk = () => new() { State = (int)ResultState.Ok };
    private static readonly Func<Error[], TBase> FactoryFailure = (Error[] errors)
        => new() { State = (int)ResultState.Error, Errors = [.. errors] };

    public static TBase NotFound() => FactoryNotFound();
    public static TBase Failure(params Error[] failures)
        => FactoryFailure(failures);
    public static TBase Ok() => FactoryOk();

    public ResultBase() : base() { }

    public ResultBase(ResultState state) : base(state) { }


    public bool IsNotFound() => State == (int)ResultState.NotFound;
    public bool IsOk() => State == (int)ResultState.Ok;
}


public abstract record ResultBase<TBase, TResult> : ResultBase<TBase>
     where TBase : ResultBase<TBase, TResult>, new()
     where TResult : class
{
    public TResult Result { get; set; } = default!;

    public ResultBase() : base(ResultState.Ok) { }

    public static TBase Ok(TResult entity)
    {
        var tbase = new TBase() { Result = entity };
        return tbase;
    }
}

public abstract record ResultIdBase<TBase, TResult> : ResultBase<TBase>
     where TBase : ResultIdBase<TBase, TResult>, new()
{
    public TResult Id { get; set; } = default!;

    public ResultIdBase() : base(ResultState.Ok) { }

    public static TBase Ok(TResult entity)
    {
        var tbase = new TBase() { Id = entity };
        return tbase;
    }
}

public enum ResultState : int
{
    /// <summary>
    /// Operation successfull
    /// </summary>
    Ok = 0x0001,
    /// <summary>
    /// Operation result - not found
    /// </summary>
    NotFound = 0x1111,
    /// <summary>
    /// General failure
    /// </summary>
    Error = 0xfffff
}
