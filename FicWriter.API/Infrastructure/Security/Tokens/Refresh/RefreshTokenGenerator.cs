using FicWriter.API.Models;
using System.Security.Cryptography;

namespace FicWriter.API.Infrastructure.Security.Tokens.Refresh;

public class RefreshTokenGenerator : IRefreshTokenGenerator
{
    private const int TokenLength = 64;
    private const int TokenLifetime = 7; // days

    public RefreshToken Generate(long userId)
    {
        return new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(TokenLength)),
            UserId = userId,
            ExpiresOnUtc = DateTime.UtcNow.AddDays(TokenLifetime)
        };
    }
}
