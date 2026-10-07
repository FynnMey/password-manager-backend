using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using PasswordManager.Api.Data;
using passwordmanagerbackend.Migrations;

namespace PasswordManager.Extensions;

public static class DatabaseExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            var connBuilder = new MySqlConnectionStringBuilder
            {
                Server = configuration["MYSQL_SERVER"],
                Port = uint.Parse(configuration["MYSQL_PORT"]!),
                Database = configuration["MYSQL_DATABASE"],
                UserID = configuration["MYSQL_USER"],
                Password = configuration["MYSQL_PASSWORD"]
            };

            var connection = connBuilder.ConnectionString;

            options.UseMySql(
                connection,
                ServerVersion.AutoDetect(connection));
        });
        
        return services;
    }
}