using FicWriter.API.Models;

namespace FicWriter.API.Features.Users.Create;

public static class CreateUserMapper
{
    public static CreateUserCommand ToCommand(this CreateUserRequest request)
    {
        return new CreateUserCommand(request.Name, request.Email, request.Password);
    }

    public static CreateUserResponse ToResponse(this User user)
    {
        return new CreateUserResponse(user.Id, user.Name);
    }

    public static User ToUser(this CreateUserCommand command, string hashedPassword)
    {
        return new User
        {
            Name = command.Name,
            Email = command.Email,
            Password = hashedPassword,
            CreatedAt = DateTime.UtcNow
        };
    }
}
