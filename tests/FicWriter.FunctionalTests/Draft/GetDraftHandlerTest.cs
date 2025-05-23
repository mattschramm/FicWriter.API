
using CommonTestUtils.Models;
using CommonTestUtils.Repositories;
using FicWriter.API.Features.Drafts.Get;
using Shouldly;

namespace FicWriter.FunctionalTests.Draft;

public class GetDraftHandlerTest
{
    private static GetDraftCommandHandler CreateHandler(API.Models.Draft? draft = null)
    {
        var draftRepositoryBuilder = new DraftRepositoryBuilder();

        if (draft is not null)
        {
            draftRepositoryBuilder.GetById(draft);
        }

        var draftRepository = draftRepositoryBuilder.Build();
        var mapper = new GetDraftMapper();

        return new GetDraftCommandHandler(draftRepository, mapper);
    }

    [Fact]
    public async Task Success()
    {
        var draft = DraftBuilder.Build();
        var handler = CreateHandler(draft);
        var command = new GetDraftCommand(draft.WorkId, draft.Id);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsError.ShouldBeFalse();
        result.Value.Title.ShouldBe(draft.Title);
        result.Value.Id.ShouldBe(draft.Id);
    }

    [Fact]
    public async Task Fail_NotFound()
    {
        var handler = CreateHandler();
        var command = new GetDraftCommand(1, 1);
        
        var result = await handler.Handle(command, CancellationToken.None);
        
        result.IsError.ShouldBeTrue();
        result.FirstError.Description.ShouldBe("Draft not found");
    }
}
