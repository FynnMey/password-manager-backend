namespace PasswordManager.Services.HttpClientService;

public interface IHttpClientService
{
    Task<byte[]> GetByteArrayAsync(string domain);
}