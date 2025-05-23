using CommonTestUtils.Models;
using CommonTestUtils.Repositories;
using CommonTestUtils.Services;
using CommonTestUtils.Tokens;
using FicWriter.API.Features.Users.Common;
using FicWriter.API.Features.Users.Login;
using Shouldly;

namespace FicWriter.FunctionalTests.User;

public class LoginHandlerTest
{
    private static LoginCommandHandler CreateHandler(API.Models.User? user = null)
    {
        var userRepositoryBuilder = new UserRepositoryBuilder();
        
        if (user is not null)
        {
            userRepositoryBuilder.GetByEmail(user);
        }
        
        var userRepository = userRepositoryBuilder.Build();
        var passwordHasher = PasswordHasherBuilder.Build();
        var tokenRepository = new TokenRepositoryBuilder().Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var accessToken = JwtTokenGeneratorBuilder.Build();
        var refreshToken = RefreshTokenGeneratorBuilder.Build();
        var mapper = new UserResponseMapper();

        return new LoginCommandHandler(
            tokenRepository,
            userRepository,
            passwordHasher,
            accessToken,
            refreshToken,
            unitOfWork,
            mapper);
    }

    [Fact]
    public async void Success()
    {
        (var user, var password) = UserBuilder.Build();
        var handler = CreateHandler(user);
        var request = new LoginCommand(user.Email, password);

        var result = await handler.Handle(request, CancellationToken.None);

        result.IsError.ShouldBeFalse();
        result.Value.Name.ShouldBe(user.Name);
        result.Value.Tokens.AccessToken.ShouldNotBeNullOrEmpty();
        result.Value.Tokens.RefreshToken.ShouldNotBeNullOrEmpty();
    }

    [Fact]
    public async void ShouldFail_InvalidCredentials()
    {
        (var user, _) = UserBuilder.Build();
        var handler = CreateHandler();
        var request = new LoginCommand(user.Email, "wrongpassword");

        var result = await handler.Handle(request, CancellationToken.None);
        
        result.IsError.ShouldBeTrue();
        result.FirstError.Code.ShouldBe("User.InvalidCredentials");
    }
}
