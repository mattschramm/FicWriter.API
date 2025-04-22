using FicWriter.API.Models;

namespace FicWriter.API.Features.Users.Auth;

public static class AuthenticateUserMapper
{
    public static AuthenticateUserCommand ToCommand(this AuthenticateUserRequest request) => new(request.RefreshToken);
}
