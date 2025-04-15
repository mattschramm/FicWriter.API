using FicWriter.API.Models;

namespace FicWriter.API.Features.Users.Shared;

public static class UserResponseMapper
{
    public static UserResponse ToResponse(this User user, string accessToken, string refreshToken) 
        => new(user.Name, new Tokens(accessToken, refreshToken));
}
