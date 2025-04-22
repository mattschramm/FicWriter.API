using CommonTestUtils.Models;
using CommonTestUtils.Repositories;
using CommonTestUtils.Tokens;
using FicWriter.API.Features.Users.Auth;
using FicWriter.API.Models;
using Shouldly;

namespace FicWriter.FunctionalTests.User;

public class AuthenticateUserHandlerTest
{
    private AuthenticateUserCommandHandler CreateHandler(RefreshToken token)
    {
        var tokenReadOnly = new TokenReadOnlyBuilder()
            .Get(token)
            .Build();

        var tokenUpdateOnly = TokenUpdateOnlyBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var accessTokenGenerator = JwtTokenGeneratorBuilder.Build();
        var refreshTokenGenerator = RefreshTokenGeneratorBuilder.Build();

        return new AuthenticateUserCommandHandler(
            tokenReadOnly,
            tokenUpdateOnly,
            unitOfWork,
            accessTokenGenerator,
            refreshTokenGenerator);
    }

    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build().user;

        var refreshToken = RefreshTokenBuilder.Build();
        refreshToken.User = user;

        var handler = CreateHandler(refreshToken);

        var command = new AuthenticateUserCommand(refreshToken.Token);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsError.ShouldBeFalse();
        result.Value.ShouldNotBeNull();
        result.Value.Tokens.AccessToken.ShouldNotBeNull();
        result.Value.Tokens.RefreshToken.ShouldNotBeNull();
        result.Value.Name.ShouldBe(user.Name);
    }

    [Fact]
    public async Task ExpiredRefreshToken()
    {
        var user = UserBuilder.Build().user;
        
        var refreshToken = RefreshTokenBuilder.Build();
        refreshToken.User = user;
        refreshToken.ExpiresOnUtc = DateTime.UtcNow.AddDays(-1);
        
        var handler = CreateHandler(refreshToken);
        
        var command = new AuthenticateUserCommand(refreshToken.Token);
        
        var result = await handler.Handle(command, CancellationToken.None);
        
        result.IsError.ShouldBeTrue();
        result.FirstError.Code.ShouldBe("User.InvalidRefreshToken");
    }
}
