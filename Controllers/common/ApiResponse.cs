namespace PasswordManager.Api.Common;

public class ApiResponse<T>
{
    public bool Success { get; init; }
    public T? Data { get; init; }
    public ApiError? Error { get; init; }

    public static ApiResponse<T> Ok(T data) =>
        new() { Success = true, Data = data };

    public static ApiResponse<T> Fail(string code, string message) =>
        new()
        {
            Success = false,
            Error = new ApiError { Code = code, Message = message }
        };
}

public class ApiResponse : ApiResponse<object>
{
    public static ApiResponse Ok() =>
        new() { Success = true };

    public static new ApiResponse Fail(string code, string message) =>
        new()
        {
            Success = false,
            Error = new ApiError { Code = code, Message = message }
        };
}

public class ApiError
{
    public string Code { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
}