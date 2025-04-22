using ErrorOr;

namespace FicWriter.API.Infrastructure.Errors;

public static class TokenErrors
{
    public static Error InvalidRefreshToken() => Error.Unauthorized(
    code: "User.InvalidRefreshToken",
    description: "Invalid refresh token.");
}
