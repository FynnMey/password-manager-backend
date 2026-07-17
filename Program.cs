using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using PasswordManager.Api.Database;
using PasswordManager.Api.Data;

DotNetEnv.Env.Load();
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connBuilder = new MySqlConnectionStringBuilder
    {
        Server = builder.Configuration["MYSQL_SERVER"],
        Port = uint.Parse(builder.Configuration["MYSQL_PORT"]!),
        Database = builder.Configuration["MYSQL_DATABASE"],
        UserID = builder.Configuration["MYSQL_USER"],
        Password = builder.Configuration["MYSQL_PASSWORD"]
    };

    var connection = connBuilder.ConnectionString;

    options.UseMySql(
        connection,
        ServerVersion.AutoDetect(connection));
});

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

await app.EnsureDatabaseCreatedAsync();

app.MapControllers();

app.Run();
