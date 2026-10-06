namespace PasswordManager.Api.Services;

public class UuidGenerator : IUuidGenerator
{
    public string NewUuid() => Guid.NewGuid().ToString();
}