using Passwordmanager.Application.Users.CreateUser;

namespace Passwordmanager.Api.Middleware;

public sealed class CreateUserRequest
{
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
}

public static class UsersEndpoints
{
    public static void MapUsersEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/users", async (CreateUserRequest req, CreateUserHandler handler) =>
        {
            var ok = await handler.HandleAsync(new CreateUserCommand(req.Username, req.Password));
            return ok ? Results.Ok(new { success = true }) : Results.BadRequest(new { success = false });
        });
    }
}
