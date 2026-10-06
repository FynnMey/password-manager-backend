using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PasswordManager.Api.Data;
using Microsoft.AspNetCore.Identity;
using PasswordManager.Api.Common;
using PasswordManager.Api.Controllers;
using PasswordManager.Api.Services;
using PasswordManager.DTOs.Auth;

namespace PasswordManager.Api.Models;

[ApiController]
[Route("api/auth")]
public class AuthController(AppDbContext db, TokenService tokens) : BaseApiController
{
    private readonly PasswordHasher<User> _hasher = new();
    private const string RefreshCookie = "refreshToken";
    
    private readonly string _errorMessageMissingToken = "Refresh token is missing";
    private readonly string _invalidCredentials = "Invalid credentials";
    
    
    private bool VerifyPassword(User user, string providedPassword)
    {
        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, providedPassword);
        return result is PasswordVerificationResult.Success or PasswordVerificationResult.SuccessRehashNeeded;
    }

    private string GetAccessToken(User user)
    {
        return tokens.GenerateAccessToken(user);
    }

    private void SetRefreshCookie(string refreshToken, DateTime expiresAt)
    {
        Response.Cookies.Append(RefreshCookie, refreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = false, // nur für test umgebung in produktion auf true setzen
            SameSite = SameSiteMode.Strict,
            Expires = new DateTimeOffset(expiresAt),
            Path = "/api/auth"
        });
    }

    private void DeleteRefreshCookie() => Response.Cookies.Delete(RefreshCookie, new CookieOptions
    {
        HttpOnly = true,
        Secure = false, // nur für test umgebung in produktion auf true setzen
        SameSite = SameSiteMode.Strict,
        Path = "/api/auth"
    });
    
    [HttpPost]
    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<AuthResponse>>> Login(GetUserRequest request)
    {
        var email = request.Email.Trim();
        var password = request.Password;

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrEmpty(password))
            return UnauthorizedUser<AuthResponse>(_invalidCredentials);
        
        var user = await db.Users
            .FirstOrDefaultAsync(b => b.Email == email);

        if (user is null || !VerifyPassword(user, password))
            return UnauthorizedUser<AuthResponse>(_invalidCredentials);

        var accessToken = GetAccessToken(user);
        var refreshToken = tokens.GenerateRefreshToken();
        var expiresAt = DateTime.UtcNow.Add(TokenService.RefreshTokenLifetime);

        db.RefreshToken.Add(new Authentification
        {
            Id = Guid.NewGuid().ToString(),
            UserId = user.Id,
            RefreshTokenHash = tokens.HashRefreshToken(refreshToken),
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = expiresAt
        });
        await db.SaveChangesAsync();
        SetRefreshCookie(refreshToken, expiresAt);

        return Success(CreateResponse(user, accessToken));
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<ApiResponse<AuthResponse>>> Refresh()
    {
        if (!Request.Cookies.TryGetValue(RefreshCookie, out var refreshToken) ||
            string.IsNullOrWhiteSpace(refreshToken))
            return UnauthorizedUser<AuthResponse>(_errorMessageMissingToken);

        var tokenHash = tokens.HashRefreshToken(refreshToken);
        var storedToken = await db.RefreshToken
            .AsNoTracking()
            .FirstOrDefaultAsync(token => token.RefreshTokenHash == tokenHash);

        if (storedToken is null || storedToken.RevokedAt is not null || storedToken.ExpiresAt <= DateTime.UtcNow)
        {
            DeleteRefreshCookie();
            return UnauthorizedUser<AuthResponse>(_errorMessageMissingToken);
        }

        var user = await db.Users.FindAsync(storedToken.UserId);
        if (user is null)
        {
            await db.RefreshToken
                .Where(token => token.Id == storedToken.Id && token.RevokedAt == null)
                .ExecuteUpdateAsync(update => update.SetProperty(token => token.RevokedAt, DateTime.UtcNow));
            DeleteRefreshCookie();
            return UnauthorizedUser<AuthResponse>(_errorMessageMissingToken);
        }

        var revokedRows = await db.RefreshToken
            .Where(token => token.Id == storedToken.Id && token.RevokedAt == null && token.ExpiresAt > DateTime.UtcNow)
            .ExecuteUpdateAsync(update => update.SetProperty(token => token.RevokedAt, DateTime.UtcNow));
        
        if (revokedRows != 1)
        {
            DeleteRefreshCookie();
            return UnauthorizedUser<AuthResponse>(_errorMessageMissingToken);
        }

        var newRefreshToken = tokens.GenerateRefreshToken();
        var expiresAt = DateTime.UtcNow.Add(TokenService.RefreshTokenLifetime);
        db.RefreshToken.Add(new Authentification
        {
            Id = Guid.NewGuid().ToString(),
            UserId = user.Id,
            RefreshTokenHash = tokens.HashRefreshToken(newRefreshToken),
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = expiresAt
        });
        await db.SaveChangesAsync();

        SetRefreshCookie(newRefreshToken, expiresAt);
        
        return Success(CreateResponse(user, GetAccessToken(user)));
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        if (Request.Cookies.TryGetValue(RefreshCookie, out var refreshToken))
        {
            var tokenHash = tokens.HashRefreshToken(refreshToken);
            var storedToken = await db.RefreshToken
                .FirstOrDefaultAsync(token => token.RefreshTokenHash == tokenHash && token.RevokedAt == null);
            if (storedToken is not null)
            {
                storedToken.RevokedAt = DateTime.UtcNow;
                await db.SaveChangesAsync();
            }
        }

        DeleteRefreshCookie();
        return Ok();
    }

    private static AuthResponse CreateResponse(User user, string accessToken)
    {
        var userDto = new UserDto(
            user.Id, 
            user.Name, 
            user.LastName, 
            user.Email, 
            user.IsPremium, 
            user.IsAdmin, 
            user.CanaryValue, 
            user.Salt
        );

        return new AuthResponse(accessToken, userDto);
    }
}
