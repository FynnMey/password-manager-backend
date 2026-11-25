using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PasswordManager.Api.Common;
using PasswordManager.Api.Controllers;
using PasswordManager.Api.Data;
using PasswordManager.Api.Models;
using PasswordManager.Services.UserService;

namespace PasswordManager.Controllers;

[ApiController]
[Route("api/admin")]
public class AdminController(
    AppDbContext db,
    IUserService userService
    ) : BaseApiController
{
    [HttpGet("is-available")]
    public async Task<ActionResult<ApiResponse<bool>>> Available()
    {
        var adminUser = await db.Users.FirstOrDefaultAsync(user => user.IsAdmin);
        
        return Success(adminUser is not null);
    }
    
    [HttpPost("create")]
    public async Task<ActionResult<ApiResponse<bool>>> Create(CreateUser request)
    {
        var (user, errorCode, errorMessage) = await userService.CreateUserAsync(request);
        
        if (user is null)
            return Failure<bool>(400, errorCode!, errorMessage!);
        
        return Success(true);
    }
}