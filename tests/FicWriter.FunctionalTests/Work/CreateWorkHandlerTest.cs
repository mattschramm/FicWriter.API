using CommonTestUtils.Models;
using CommonTestUtils.Repositories;
using CommonTestUtils.Repositories.Works;
using CommonTestUtils.Requests;
using CommonTestUtils.Services;
using FicWriter.API.Features.Works.Create;
using Shouldly;

namespace FicWriter.FunctionalTests.Work;

public class CreateWorkHandlerTest
{
    private static CreateWorkCommandHandler CreateHandler(API.Models.User user)
    {
        var workWriteOnly = WorkWriteOnlyBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var currentUser = CurrentUserBuilder.Build(user);
        var mapper = new CreateWorkMapper(SqidsEncoderBuilder.Build());

        return new CreateWorkCommandHandler(
            workWriteOnly,
            unitOfWork,
            currentUser,
            mapper);
    }

    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build().user;
        var request = CreateWorkCommandBuilder.Build();
        var handler = CreateHandler(user);

        var result = await handler.Handle(request, CancellationToken.None);

        result.IsError.ShouldBeFalse();
        result.Value.ShouldNotBeNull();
        result.Value.Title.ShouldBe(request.Title);
    }
}
