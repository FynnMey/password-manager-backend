using System.Net;

namespace PasswordManager.Api.Services;

public class HttpClientService
{
    private static readonly HttpClient Client = new HttpClient();

    public async Task<byte[]> GetByteArrayAsync(string domain)
    {
        return await Client.GetByteArrayAsync(domain);
    }
}