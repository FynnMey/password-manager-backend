using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PasswordManager.Api.Common;
using PasswordManager.Api.Database;
using PasswordManager.Api.Models;
using PasswordManager.Api.Services;
using PasswordManager.Services.CurrentUserService;
using PasswordManager.Services.PasswordService;

namespace PasswordManager.Api.Controllers;

[ApiController]
[Route("api/user/password")]
public class PasswordController(AppDbContext db, ICurrentUserService currentUserService, IPasswordService passwordService) : BaseApiController
{
    [Authorize]
    [HttpPost("create")]
    public async Task<ActionResult<ApiResponse<Vault>>> Create(CreatePassword request)
    {
        if (string.IsNullOrEmpty(currentUserService.UserId))
            return UnauthorizedUser<Vault>();

        var (vault, errorCode, errorMessage) = await passwordService.CreateCurrentUserVault(request);
        
        if (vault is null)
            return Failure<Vault>(400, errorCode!, errorMessage!);
        
        return Success(vault);
    }

    [Authorize]
    [HttpPost("delete")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(DeletePassword request)
    {
        if (string.IsNullOrEmpty(currentUserService.UserId))
            return UnauthorizedUser<bool>();
        
        var (isSuccess, errorCode, errorMessage) = await passwordService.DeletePasswordByCurrentUserAsync(request.Id);

        if (isSuccess == false)
            return Failure<bool>(400, errorCode!, errorMessage!);
        
        return Success(true);
    }
    
    [Authorize]
    [HttpPost("update")]
    public async Task<ActionResult<ApiResponse<Vault>>> Update(UpdatePassword request)
    {
        if (string.IsNullOrEmpty(currentUserService.UserId))
            return UnauthorizedUser<Vault>();
    
        var (vault, errorCode, errorMessage) = await passwordService.UpdatePasswordByCurrentUserAsync(request);

        if (vault is null)
            return Failure<Vault>(400, errorCode!, errorMessage!);
    
        return Success(vault);
    }

    [Authorize]
    [HttpPost("get-all")]
    public async Task<ActionResult<ApiResponse<List<Vault>>>> GetAllFromUser()
    {
        if (string.IsNullOrEmpty(currentUserService.UserId))
            return UnauthorizedUser<List<Vault>>();

        var (userVault, errorCode, errorMessage) = await passwordService.GetVaultByCurrentUserAsync();
        
        if (userVault is null)
            return Failure<List<Vault>>(400, errorCode!, errorMessage!);
        return Success(userVault);
        
    }
}