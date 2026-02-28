using Passwordmanager.Api.Middleware;
using Passwordmanager.Application.Users.CreateUser;
using Microsoft.EntityFrameworkCore;
using Passwordmanager.Application.Interfaces;
using Passwordmanager.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<CreateUserHandler>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<IAppDbContext, AppDbContext>();

var app = builder.Build();

app.MapGet("/hello-world", () => "Hello World!");
app.MapUsersEndpoints();

app.Run();
