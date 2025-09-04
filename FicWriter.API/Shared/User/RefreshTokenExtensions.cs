using FicWriter.API.Models;

namespace FicWriter.API.Shared.User;

public static class RefreshTokenExtensions
{
    public static bool IsExpired(this RefreshToken token) => token.ExpiresOnUtc < DateTime.UtcNow;
}
