using CommonTestUtils.Repositories;
using CommonTestUtils.Requests;
using CommonTestUtils.Services;
using CommonTestUtils.Tokens;
using FicWriter.API.Features.Users.Create;
using Shouldly;

namespace FicWriter.FunctionalTests.User;

public class CreateUserHandlerTest
{
    private static CreateUserCommandHandler CreateHandler(string? email = null)
    {
        var userReadOnlyBuilder = new UserReadOnlyBuilder();

        if (!string.IsNullOrEmpty(email))
        {
            userReadOnlyBuilder.ExistsWithEmail(email);
        }

        var userReadOnly = userReadOnlyBuilder.Build();
        var userWriteOnly = UserWriteOnlyBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var passwordHasher = PasswordHasherBuilder.Build();
        var accessToken = JwtTokenGeneratorBuilder.Build();
        var refreshToken = RefreshTokenGeneratorBuilder.Build();
        var tokenWriteOnly = TokenWriteOnlyBuilder.Build();

        return new CreateUserCommandHandler(
            userReadOnly,
            userWriteOnly,
            unitOfWork,
            passwordHasher,
            accessToken,
            refreshToken,
            tokenWriteOnly);
    }

    [Fact]
    public async Task Success()
    {
        var request = CreateUserRequestBuilder.Build();
        var handler = CreateHandler();

        var result = await handler.Handle(request.ToCommand(), CancellationToken.None);

        result.IsError.ShouldBeFalse();
        result.Value.ShouldNotBeNull();
        result.Value.Name.ShouldBe(request.Name);
    }

    [Fact]
    public async Task ShouldFail_EmailAlreadyExists()
    {
        var request = CreateUserRequestBuilder.Build();
        var handler = CreateHandler(request.Email);

        var result = await handler.Handle(request.ToCommand(), CancellationToken.None);

        result.IsError.ShouldBeTrue();
        result.FirstError.Code.ShouldBe("User.EmailAlreadyExists");
        result.FirstError.Description.ShouldBe($"User with email {request.Email} already exists.");
    }
}
