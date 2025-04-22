using CommonTestUtils.Models;
using CommonTestUtils.Repositories;
using CommonTestUtils.Services;
using CommonTestUtils.Tokens;
using FicWriter.API.Features.Users.Login;
using FicWriter.API.Models;
using Shouldly;

namespace FicWriter.FunctionalTests.User;

public class LoginHandlerTest
{
    private static LoginCommandHandler CreateHandler(API.Models.User? user = null)
    {
        var userReadOnlyBuilder = new UserReadOnlyBuilder();
        
        if (user is not null)
        {
            userReadOnlyBuilder.GetByEmail(user);
        }
        
        var userReadOnly = userReadOnlyBuilder.Build();
        var passwordHasher = PasswordHasherBuilder.Build();
        var tokenWriteOnly = TokenWriteOnlyBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var accessToken = JwtTokenGeneratorBuilder.Build();
        var refreshToken = RefreshTokenGeneratorBuilder.Build();

        return new LoginCommandHandler(userReadOnly, passwordHasher, accessToken, refreshToken, tokenWriteOnly, unitOfWork);
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
