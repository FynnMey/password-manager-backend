using PasswordManager.Api.Services;

namespace PasswordManager.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<IUuidGenerator, UuidGenerator>();

        return services;
    }
}