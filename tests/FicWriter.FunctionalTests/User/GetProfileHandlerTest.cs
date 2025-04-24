using CommonTestUtils.Models;
using CommonTestUtils.Services;
using FicWriter.API.Features.Users.Profile;
using Shouldly;

namespace FicWriter.FunctionalTests.User;

public class GetProfileHandlerTest
{
    private static GetProfileCommandHandler CreateHandler(API.Models.User user)
    {
        var currentUser = CurrentUserBuilder.Build(user);

        return new GetProfileCommandHandler(currentUser);
    }

    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build().user;

        var handler = CreateHandler(user);

        var result = await handler.Handle(new GetProfileCommand(), CancellationToken.None);

        result.IsError.ShouldBeFalse();
    }
}
