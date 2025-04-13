using FicWriter.API.Infrastructure.Security.Tokens.Generator;

namespace CommonTestUtils.Tokens;

public static class JwtTokenGeneratorBuilder
{
    public static IAccessTokenGenerator Build(
        string key = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
        uint expirationTime = 1000,
        string issuer = "testissuer",
        string audience = "testaudience") => new JwtTokenGenerator(key, expirationTime, issuer, audience);
}
