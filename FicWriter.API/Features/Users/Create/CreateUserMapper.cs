using FicWriter.API.Features.Users.Common;
using FicWriter.API.Models;

namespace FicWriter.API.Features.Users.Create;

public class CreateUserMapper : UserResponseMapper
{
    public User ToUser(CreateUserCommand command, string hashedPassword)
    {
        return new User
        {
            Name = command.Name,
            Email = command.Email,
            Password = hashedPassword,
            CreatedAt = DateTime.UtcNow,
        };
    }
}
