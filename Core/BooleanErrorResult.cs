namespace Core;

public class BooleanErrorResult
{
    public bool Result { get; set; }
    public string Information { get; set; }
    public string Key { get; set; }
    public int ErrorCode { get; set; }

    public BooleanErrorResult(bool result, string information = null, string key = null, int errorCode = 200)
    {
        Result = result;
        Information = information;
        //It used to get message from result
        Key = key;
        ErrorCode = errorCode;
    }
}

public class BooleanErrorResult<T> : BooleanErrorResult
{
    public T Data { get; set; }

    public BooleanErrorResult(bool result, string information, T data, int errorCode = 200)
        : base(result, information, null, errorCode)
    {
        Data = data;
    }
}

public class BooleanErrorResult<TData, TError> : BooleanErrorResult
{
    public TData Data { get; set; }
    public TError Error { get; set; }

    public BooleanErrorResult(bool result, string information, TData data = default, int errorCode = 200,
        TError error = default)
        : base(result, information, null, errorCode)
    {
        Data = data;
        Error = error;
    }
}