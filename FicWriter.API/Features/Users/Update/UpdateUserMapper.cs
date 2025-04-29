namespace FicWriter.API.Features.Users.Update;

public static class UpdateUserMapper
{
    public static UpdateUserCommand ToCommand(this UpdateUserRequest request) => new(request.Name, request.Email);
}
