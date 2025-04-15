using FicWriter.API.Features.Users.Shared;
using FicWriter.API.Models;

namespace FicWriter.API.Features.Users.Login;

public static class LoginMapper
{
    public static LoginCommand ToCommand(this LoginRequest request) => new(request.Email, request.Password);
}
