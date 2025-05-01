using CommonTestUtils.Models;
using CommonTestUtils.Repositories;
using CommonTestUtils.Requests;
using CommonTestUtils.Services;
using ErrorOr;
using FicWriter.API.Features.Users.Update;
using FicWriter.API.Infrastructure.Errors;
using Shouldly;

namespace FicWriter.FunctionalTests.User;

public class UpdateUserHandlerTest
{
    private static UpdateUserCommandHandler CreateHandler(API.Models.User user, string? email = null)
    {
        var userUpdateOnly = new UserUpdateOnlyBuilder().GetUserByIdWithTracking(user).Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var currentUser = CurrentUserBuilder.Build(user);

        var userReadOnlyBuilder = new UserReadOnlyBuilder();

        if (!string.IsNullOrEmpty(email))
        {
            userReadOnlyBuilder.ExistsWithEmail(email);
        }

        return new UpdateUserCommandHandler(
            userUpdateOnly,
            unitOfWork,
            currentUser,
            userReadOnlyBuilder.Build());
    }

    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build().user;
        var handler = CreateHandler(user);
        var request = UpdateUserCommandBuilder.Build();

        var result = await handler.Handle(request, CancellationToken.None);

        result.IsError.ShouldBeFalse();
        result.Value.ShouldBe(Result.Success);
    }

    [Fact]
    public async Task Fail_EmailAlreadyExists()
    {
        var user = UserBuilder.Build().user;
        var request = UpdateUserCommandBuilder.Build();
        var handler = CreateHandler(user, request.Email);

        var result = await handler.Handle(request, CancellationToken.None);
        
        result.IsError.ShouldBeTrue();
        result.FirstError.ShouldBe(UserErrors.EmailAlreadyExists());
    }
}
