using FicWriter.API.Models;

namespace FicWriter.API.Features.Users.Common;

public static class RefreshTokenExtensions
{
    public static bool IsExpired(this RefreshToken token) => token.ExpiresOnUtc < DateTime.UtcNow;
}
