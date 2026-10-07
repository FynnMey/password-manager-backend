namespace PasswordManager.Services.HttpClientService;

public class HttpClientService : IHttpClientService
{
    private static readonly HttpClient Client = new HttpClient();

    public async Task<byte[]> GetByteArrayAsync(string url)
    {
        return await Client.GetByteArrayAsync(url);
    }
}