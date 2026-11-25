using PasswordManager.Api.Services;
using PasswordManager.Services.HttpClientService;
using PasswordManager.Services.PasswordHasherService;
using PasswordManager.Services.TokenService;
using PasswordManager.Services.UserService;

namespace PasswordManager.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<IUuidGenerator, UuidGenerator>();
        services.AddSingleton<IHttpClientService, HttpClientService>();
        services.AddSingleton<ITokenService, TokenService>();
        services.AddSingleton<IPasswordHasherService, PasswordHasherService>();
        services.AddSingleton<IUserService, UserService>();

        return services;
    }
}