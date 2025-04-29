using ErrorOr;

namespace FicWriter.API.Infrastructure.Errors;

public static class UserErrors
{
    public static Error EmailAlreadyExists() =>
        Error.Conflict(
            code: "User.EmailAlreadyExists",
            description: $"User with provided email already exists.");

    public static Error InvalidCredentials() =>
        Error.Unauthorized(
            code: "User.InvalidCredentials",
            description: "Invalid email or password.");
}
