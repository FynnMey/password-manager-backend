using Microsoft.AspNetCore.Mvc;
using PasswordManager.Api.Common;

namespace PasswordManager.Api.Controllers;

[ApiController]
public abstract class BaseApiController : ControllerBase
{
    protected ActionResult<ApiResponse<T>> Success<T>(T data)
    {
        return Ok(ApiResponse<T>.Ok(data));
    }

    protected ActionResult<ApiResponse<T>> Failure<T>(int statusCode, string code, string message)
    {
        return StatusCode(statusCode, ApiResponse<T>.Fail(code, message));
    }
    
    protected ActionResult<ApiResponse<T>> UnauthorizedUser<T>()
    {
        return Failure<T>(401, "UNAUTHORIZED", "No user could be found in the token.");
    }
}