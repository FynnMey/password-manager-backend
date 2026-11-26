using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Api.Models;
using PasswordManager.Api.Common;
using PasswordManager.Api.Database;
using PasswordManager.Services.UserService;

namespace PasswordManager.Api.Controllers;

[ApiController]
[Route("api/user")]
public class UserController(IUserService userService) : BaseApiController
{
    [Authorize]
    [HttpPost("register")]
    public ActionResult<ApiResponse<bool>> Register(RegisterUser request)
    {
        if (string.IsNullOrWhiteSpace(request.CanaryValue))
            return Failure<bool>(400, "INVALID_CANARY_VALUE", "The canary value is missing or invalid.");
        
        if (string.IsNullOrWhiteSpace(request.Salt))
            return Failure<bool>(400, "INVALID_SALT", "The salt is missing or invalid.");

        var isSuccess = userService.UpdateSaltAndCarnary(request);

        return Success(isSuccess);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("create")]
    public async Task<ActionResult<ApiResponse<User>>> Create(CreateUser request)
    {
        var (user, errorCode, errorMessage) = await userService.CreateUserAsync(request);

        if (user is null)
            return Failure<User>(400, errorCode!, errorMessage!);

        return Success(user);
    }
}
