using FicWriter.API.Shared.Mapper;

namespace FicWriter.API.Features.Users.Common;

public class UserResponseMapper : IFeatureMapper
{
    public UserResponse ToResponse(Models.User user, string accessToken, string refreshToken) 
        => new(user.Name, new Tokens(accessToken, refreshToken));
}
