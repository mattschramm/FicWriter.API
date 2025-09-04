using FicWriter.API.Infrastructure.Security.Tokens.Refresh;

namespace CommonTestUtils.Tokens;

public static class RefreshTokenGeneratorBuilder
{
    public static IRefreshTokenGenerator Build() => new RefreshTokenGenerator();
}
