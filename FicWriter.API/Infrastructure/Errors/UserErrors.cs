using ErrorOr;

namespace FicWriter.API.Infrastructure.Errors;

public static class UserErrors
{
    public static Error EmailAlreadyExists(string email) =>
        Error.Conflict(
            code: "User.EmailAlreadyExists",
            description: $"User with email {email} already exists.");

    public static Error InvalidCredentials() =>
        Error.Unauthorized(
            code: "User.InvalidCredentials",
            description: "Invalid email or password.");
}
