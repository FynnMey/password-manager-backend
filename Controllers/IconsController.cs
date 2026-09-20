using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PasswordManager.Api.Common;
using PasswordManager.Api.Data;
using PasswordManager.Api.Models;
using PasswordManager.Api.Services;

namespace PasswordManager.Api.Controllers;

[ApiController]
[Route("api/icon")]
public class IconsController(AppDbContext db) : BaseApiController
{
    
    // POST api/icon
    [HttpPost]
    public async Task<ActionResult<ApiResponse<string>>> GetIconForUrl(GetIconRequest request)
    {
        if (!Uri.TryCreate(request.Url, UriKind.Absolute, out var uri))
            return Failure<string>(500, "Server Error", "Url is not valid.");

        var baseUrl = uri.Host;
        Console.WriteLine(baseUrl);

        var existingIcon = await db.Icons.FirstOrDefaultAsync(icon => icon.Url == baseUrl);

        if (existingIcon != null)
            return Success(existingIcon.Image); 
        

        var client = new HttpClientService();
        var data = null as byte[];

        try
        {
            data = await client.GetByteArrayAsync("https://www.google.com/s2/favicons?domain=" + baseUrl + "&sz=128");
        }
        catch
        {
            return Failure<string>(502, "IconFetchFailed", "Could not retrieve icon for domain '" + baseUrl + "' from external provider.");
        }

        var image = string.Concat("data:image/png;base64,", Convert.ToBase64String(data));

        var icon = new Icon
        {
            Url = baseUrl,
            Image = image
        };

        db.Icons.Add(icon);
        await db.SaveChangesAsync();

        return Success(image);
    }
}