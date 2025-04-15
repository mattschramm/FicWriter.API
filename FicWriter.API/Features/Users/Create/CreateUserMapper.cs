using FicWriter.API.Models;

namespace FicWriter.API.Features.Users.Create;

public static class CreateUserMapper
{
    public static CreateUserCommand ToCommand(this CreateUserRequest request) => new(request.Name, request.Email, request.Password);

    public static User ToUser(this CreateUserCommand command, string hashedPassword)
    {
        return new User
        {
            Name = command.Name,
            Email = command.Email,
            Password = hashedPassword,
            CreatedAt = DateTime.UtcNow,
            UserIdentifier = Guid.NewGuid()
        };
    }
}
