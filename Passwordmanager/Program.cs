using Passwordmanager.Api.Middleware;
using Passwordmanager.Application.Users.CreateUser;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<CreateUserHandler>();

var app = builder.Build();

app.MapGet("/hello-world", () => "Hello World!");
app.MapUsersEndpoints();

app.Run();
