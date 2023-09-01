using Faliush.ContactManager.Core.Common.OperationResult.Base;

namespace Faliush.ContactManager.Core.Common.OperationResult;

[Serializable]
public abstract class OperationResultBase
{
    public Metadata? Metadata { get; set; }
    public Exception? Exception { get; set; }

    public static OperationResult<TResult> CreateResult<TResult>(TResult result, Exception? exception = null)
    {
        var operation = new OperationResult<TResult>
        {
            Result = result,
            Exception = exception
        };
        return operation;
    }

    public static OperationResult<TResult> CreateResult<TResult>() => CreateResult(default(TResult)!);
}

[Serializable]
public class OperationResult<T> : OperationResultBase
{
    public T? Result { get; set; }

    public IHaveData? AddInfo(string message)
    {
        Metadata = new Metadata(this, message, MetadataTypes.Info);
        return Metadata;
    }

    public IHaveData? AddSuccess(string message)
    {
        Metadata = new Metadata(this, message, MetadataTypes.Success);
        return Metadata;
    }

    public IHaveData? AddWarning(string message)
    {
        Metadata = new Metadata(this, message, MetadataTypes.Warning);
        return Metadata;
    }

    public IHaveData? AddError(string message)
    {
        Metadata = new Metadata(this, message, MetadataTypes.Error);
        return Metadata;
    }

    public IHaveData? AddError(string message, Exception exception)
    {
        if(exception is not null)
            Exception = exception;

        Metadata = new Metadata(this, message, MetadataTypes.Error);
        return Metadata;
    }

    public IHaveData? AddError(Exception? exception)
    {
        if (exception is not null)
            Exception = exception;

        Metadata = new Metadata(this, "Something went wrong", MetadataTypes.Error);
        return Metadata;
    }

    public bool Ok 
    {
        get
        {
            if (Metadata is null)
                return Exception is null && Result is not null;

            return Metadata.Type is not MetadataTypes.Error
                && Result is not null
                && Exception is null;
        }
    }
}
