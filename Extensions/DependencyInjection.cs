using PasswordManager.Api.Services;
using PasswordManager.Services.HttpClientService;
using PasswordManager.Services.PasswordHasherService;
using PasswordManager.Services.TokenService;

namespace PasswordManager.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<IUuidGenerator, UuidGenerator>();
        services.AddSingleton<IHttpClientService, HttpClientService>();
        services.AddSingleton<ITokenService, TokenService>();
        services.AddSingleton<IPasswordHasherService, PasswordHasherService>();

        return services;
    }
}