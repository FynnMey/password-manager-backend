using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MySqlConnector;
using PasswordManager.Api.Database;
using PasswordManager.Api.Data;
using System.Text;

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

var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET")
    ?? throw new InvalidOperationException("JWT_SECRET is missing in environment variables.");

builder.Services.AddSingleton<PasswordManager.Api.Services.TokenService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
            ValidateIssuer = true,
            ValidIssuer = PasswordManager.Api.Services.TokenService.Issuer,
            ValidateAudience = true,
            ValidAudience = PasswordManager.Api.Services.TokenService.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

await app.EnsureDatabaseCreatedAsync();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
