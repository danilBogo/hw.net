namespace TimMovie.SharedKernel.Classes;

public class Result
{
    public Result(bool succeeded, string error)
    {
        switch (succeeded)
        {
            case true when error != string.Empty:
                throw new InvalidOperationException();
            case false when error == string.Empty:
                throw new InvalidOperationException();
            default:
                Succeeded = succeeded;
                Error = error;
                break;
        }
    }

    public bool Succeeded { get; }
    public string Error { get; }
    
    public bool IsFailure => !Succeeded;

    public static Result Fail(string message)
    {
        return new Result(false, message);
    }

    public static Result<T> Fail<T>(string message)
    {
        return new Result<T>(default, false, message);
    }

    public static Result Ok()
    {
        return new Result(true, string.Empty);
    }

    public static Result<T> Ok<T>(T value)
    {
        return new Result<T>(value, true, string.Empty);
    }
}

public class Result<T> : Result
{
    public Result(T value, bool succeeded, string error)
        : base(succeeded, error)
    {
        Value = value;
    }
    
    public T Value { get; set; }
}