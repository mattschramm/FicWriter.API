using CommonTestUtils.Models;
using CommonTestUtils.Repositories;
using CommonTestUtils.Requests;
using CommonTestUtils.Services;
using FicWriter.API.Features.Drafts.Create;
using Shouldly;

namespace FicWriter.FunctionalTests.Draft;

public class CreateDraftHandlerTest
{
    private static CreateDraftCommandHandler CreateHandler(API.Models.User user, API.Models.Work? work = null)
    {
        var workRepositoryBuilder = new WorkRepositoryBuilder();
        
        if (work != null)
        {
            workRepositoryBuilder.Exists(work, user);
        }

        var draftRepository = new DraftRepositoryBuilder().Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var currentUser = CurrentUserBuilder.Build(user);
        var workRepository = workRepositoryBuilder.Build();
        var mapper = new CreateDraftMapper();

        return new CreateDraftCommandHandler(
            draftRepository,
            unitOfWork,
            workRepository,
            currentUser,
            mapper
        );
    }

    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build().user;
        var work = WorkBuilder.Build(user);
        var command = CreateDraftCommandBuilder.Build();
        var handler = CreateHandler(user, work);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsError.ShouldBeFalse();
        result.Value.Title.ShouldBe(command.Title);
        result.Value.Order.ShouldBe(command.Order);
    }

    [Fact]
    public async Task Fail_WorkNotFound()
    {
        var user = UserBuilder.Build().user;
        var command = CreateDraftCommandBuilder.Build();
        var handler = CreateHandler(user);
        
        var result = await handler.Handle(command, CancellationToken.None);
        
        result.IsError.ShouldBeTrue();
    }
}
