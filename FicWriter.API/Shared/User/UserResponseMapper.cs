namespace FicWriter.API.Shared.User;

public static class UserResponseMapper
{
    public static UserResponse ToResponse(this Models.User user, string accessToken, string refreshToken) 
        => new(user.Name, new Tokens(accessToken, refreshToken));
}
