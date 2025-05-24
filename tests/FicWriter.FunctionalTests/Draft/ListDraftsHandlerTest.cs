using CommonTestUtils.Models;
using CommonTestUtils.Repositories;
using FicWriter.API.Features.Drafts.List;
using Shouldly;

namespace FicWriter.FunctionalTests.Draft;

public class ListDraftsHandlerTest
{
    private static ListDraftsCommandHandler CreateHandler(List<API.Models.Draft> drafts)
    {
        var draftRepository = new DraftRepositoryBuilder().GetDrafts(1, drafts).Build();
        var mapper = new ListDraftsMapper();

        return new ListDraftsCommandHandler(draftRepository, mapper);
    }

    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build().user;
        var work = WorkBuilder.Build(user);
        var drafts = DraftBuilder.BuildList(work);
        var handler = CreateHandler(drafts);
        var command = new ListDraftsCommand(1);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsError.ShouldBeFalse();
        result.Value.Drafts.Count.ShouldBe(drafts.Count);
    }
}
