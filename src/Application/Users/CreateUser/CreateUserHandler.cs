namespace Passwordmanager.Application.Users.CreateUser;

public sealed class CreateUserHandler
{
    public Task<bool> HandleAsync(CreateUserCommand command, CancellationToken ct = default)
    {
        Console.WriteLine($"Hey user {command.Username} angelegt");
        return Task.FromResult(true);
    }
}
