using FicWriter.API.Models;

namespace FicWriter.API.Features.Users.Create;

public static class CreateUserMapper
{
    public static CreateUserCommand ToCommand(this CreateUserRequest request) => new(request.Name, request.Email, request.Password);

    public static CreateUserResponse ToResponse(this User user, string token) => new(user.Id, user.Name, new AccessToken { Token = token });

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
