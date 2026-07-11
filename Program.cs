using Microsoft.EntityFrameworkCore;
using PasswordManager.Api.Database;
using PasswordManager.Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connection =
        builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is missing.");

    options.UseMySql(
        connection,
        ServerVersion.AutoDetect(connection));
});

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

await app.EnsureDatabaseCreatedAsync();

app.MapControllers();

app.Run();
