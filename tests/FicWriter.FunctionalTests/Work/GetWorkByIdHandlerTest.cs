using CommonTestUtils.Models;
using CommonTestUtils.Repositories;
using CommonTestUtils.Services;
using FicWriter.API.Features.Works.Get;
using Shouldly;

namespace FicWriter.FunctionalTests.Work;

public class GetWorkByIdHandlerTest
{
    private static GetWorkByIdCommandHandler CreateHandler(API.Models.User user, API.Models.Work? work = null)
    {
        var workRepositoryBuilder = new WorkRepositoryBuilder();

        if (work is not null)
        {
            workRepositoryBuilder.GetById(work, user);
        }

        var workRepository = workRepositoryBuilder.Build();
        var currentUser = CurrentUserBuilder.Build(user);
        var mapper = new GetWorkByIdMapper(SqidsEncoderBuilder.Build());

        return new GetWorkByIdCommandHandler(workRepository, currentUser, mapper);
    }

    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build().user;
        var work = WorkBuilder.Build(user);
        var handler = CreateHandler(user, work);
        var command = new GetWorkByIdCommand(work.Id);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsError.ShouldBeFalse();
        result.Value.Title.ShouldBe(work.Title);
        result.Value.Description.ShouldBe(work.Description);
    }

    [Fact]
    public async Task Fail_WorkNotFound()
    {
        var user = UserBuilder.Build().user;
        var work = WorkBuilder.Build(user);
        var handler = CreateHandler(user);
        var command = new GetWorkByIdCommand(work.Id);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsError.ShouldBeTrue();
        result.FirstError.Description.ShouldBe("Work not found");
    }
}
