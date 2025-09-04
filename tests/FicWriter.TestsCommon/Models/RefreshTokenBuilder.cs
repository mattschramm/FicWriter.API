using FicWriter.API.Models;

namespace CommonTestUtils.Models;

public static class RefreshTokenBuilder
{
    public static RefreshToken Build() => new()
    {
        Id = Guid.NewGuid(),
        Token = "sample-refresh-token",
        UserId = 1,
        ExpiresOnUtc = DateTime.UtcNow.AddDays(7),
    };
}
