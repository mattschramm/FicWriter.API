using CommonTestUtils.Models;
using CommonTestUtils.Repositories;
using CommonTestUtils.Requests;
using FicWriter.API.Features.Drafts.Create;
using Shouldly;

namespace FicWriter.FunctionalTests.Draft;

public class CreateDraftHandlerTest
{
    private static CreateDraftCommandHandler CreateHandler(API.Models.User user, API.Models.Work? work = null)
    {
        var workRepositoryBuilder = new WorkRepositoryBuilder();
        var draftRepositoryBuilder = new DraftRepositoryBuilder();

        if (work != null)
        {
            workRepositoryBuilder.Exists(work, user);
            draftRepositoryBuilder.GetNextOrder(work.Id, 1);
        }

        
        var unitOfWork = UnitOfWorkBuilder.Build();
        var draftRepository = draftRepositoryBuilder.Build();
        var mapper = new CreateDraftMapper();

        return new CreateDraftCommandHandler(
            draftRepository,
            unitOfWork,
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
    }
}
