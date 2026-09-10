using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PasswordManager.Api.Data;
using Microsoft.AspNetCore.Identity;
using PasswordManager.Api.Services;

namespace PasswordManager.Api.Models;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly TokenService _tokens;
    private readonly PasswordHasher<User> _hasher = new();
    private const string RefreshCookie = "refreshToken";
    
    private readonly string _errorMessageMissingToken = "Refresh token is missing";
    private readonly string _invalidCredentials = "Invalid credentials";
    
    public AuthController(AppDbContext db, TokenService tokens)
    {
        _db = db;
        _tokens = tokens;
    }
    
    
    private bool VerifyPassword(User user, string providedPassword)
    {
        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, providedPassword);
        return result is PasswordVerificationResult.Success or PasswordVerificationResult.SuccessRehashNeeded;
    }

    private string GetAccessToken(User user)
    {
        return _tokens.GenerateAccessToken(user);
    }

    private void SetRefreshCookie(string refreshToken, DateTime expiresAt)
    {
        Response.Cookies.Append(RefreshCookie, refreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = new DateTimeOffset(expiresAt),
            Path = "/api/auth"
        });
    }

    private void DeleteRefreshCookie() => Response.Cookies.Delete(RefreshCookie, new CookieOptions
    {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.Strict,
        Path = "/api/auth"
    });
    
    // POST api/auth
    [HttpPost]
    [HttpPost("login")]
    public async Task<ActionResult> Login(GetUserRequest request)
    {
        var email = request.Email.Trim();
        var password = request.Password;

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrEmpty(password))
            return Unauthorized(new { message = _invalidCredentials });
        
        var user = await _db.Users
            .FirstOrDefaultAsync(b => b.Email == email);

        if (user is null || !VerifyPassword(user, password))
            return Unauthorized(new { message = _invalidCredentials });

        var accessToken = GetAccessToken(user);
        var refreshToken = _tokens.GenerateRefreshToken();
        var expiresAt = DateTime.UtcNow.Add(TokenService.RefreshTokenLifetime);

        _db.RefreshToken.Add(new Authentification
        {
            Id = Guid.NewGuid().ToString(),
            UserId = user.Id,
            RefreshTokenHash = _tokens.HashRefreshToken(refreshToken),
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = expiresAt
        });
        await _db.SaveChangesAsync();
        SetRefreshCookie(refreshToken, expiresAt);

        return Ok(CreateResponse(user, accessToken));
    }

    [HttpPost("refresh")]
    public async Task<ActionResult> Refresh()
    {
        if (!Request.Cookies.TryGetValue(RefreshCookie, out var refreshToken) ||
            string.IsNullOrWhiteSpace(refreshToken))
            return Unauthorized(new { message = _errorMessageMissingToken });

        var tokenHash = _tokens.HashRefreshToken(refreshToken);
        var storedToken = await _db.RefreshToken
            .AsNoTracking()
            .FirstOrDefaultAsync(token => token.RefreshTokenHash == tokenHash);

        if (storedToken is null || storedToken.RevokedAt is not null || storedToken.ExpiresAt <= DateTime.UtcNow)
        {
            DeleteRefreshCookie();
            return Unauthorized(new { message = _errorMessageMissingToken });
        }

        var user = await _db.Users.FindAsync(storedToken.UserId);
        if (user is null)
        {
            await _db.RefreshToken
                .Where(token => token.Id == storedToken.Id && token.RevokedAt == null)
                .ExecuteUpdateAsync(update => update.SetProperty(token => token.RevokedAt, DateTime.UtcNow));
            DeleteRefreshCookie();
            return Unauthorized(new { message = _errorMessageMissingToken });
        }

        var revokedRows = await _db.RefreshToken
            .Where(token => token.Id == storedToken.Id && token.RevokedAt == null && token.ExpiresAt > DateTime.UtcNow)
            .ExecuteUpdateAsync(update => update.SetProperty(token => token.RevokedAt, DateTime.UtcNow));
        if (revokedRows != 1)
        {
            DeleteRefreshCookie();
            return Unauthorized(new { message = _errorMessageMissingToken });
        }

        var newRefreshToken = _tokens.GenerateRefreshToken();
        var expiresAt = DateTime.UtcNow.Add(TokenService.RefreshTokenLifetime);
        _db.RefreshToken.Add(new Authentification
        {
            Id = Guid.NewGuid().ToString(),
            UserId = user.Id,
            RefreshTokenHash = _tokens.HashRefreshToken(newRefreshToken),
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = expiresAt
        });
        await _db.SaveChangesAsync();

        SetRefreshCookie(newRefreshToken, expiresAt);
        return Ok(CreateResponse(user, GetAccessToken(user)));
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        if (Request.Cookies.TryGetValue(RefreshCookie, out var refreshToken))
        {
            var tokenHash = _tokens.HashRefreshToken(refreshToken);
            var storedToken = await _db.RefreshToken
                .FirstOrDefaultAsync(token => token.RefreshTokenHash == tokenHash && token.RevokedAt == null);
            if (storedToken is not null)
            {
                storedToken.RevokedAt = DateTime.UtcNow;
                await _db.SaveChangesAsync();
            }
        }

        DeleteRefreshCookie();
        return Ok();
    }

    private static object CreateResponse(User user, string accessToken) => new
    {
        accessToken,
        user = new { user.Id, user.Name, user.LastName, user.Email, user.IsPremium, user.IsAdmin, user.CanaryValue, user.Salt }
    };
}
