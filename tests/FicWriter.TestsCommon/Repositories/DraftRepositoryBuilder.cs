using FicWriter.API.Infrastructure.Data.Repositories.Drafts;
using FicWriter.API.Models;
using Moq;

namespace CommonTestUtils.Repositories;

public class DraftRepositoryBuilder
{
    private readonly Mock<IDraftRepository> _draftRepositoryMock;

    public DraftRepositoryBuilder()
    {
        _draftRepositoryMock = new Mock<IDraftRepository>();
    }

    public IDraftRepository Build() => _draftRepositoryMock.Object;

    public DraftRepositoryBuilder GetById(Draft draft)
    {
        _draftRepositoryMock
            .Setup(x => x.GetDraftById(draft.WorkId, draft.Id))
            .ReturnsAsync(draft);

        return this;
    }

    public DraftRepositoryBuilder GetNextOrder(long workId, uint order)
    {
        _draftRepositoryMock
            .Setup(x => x.GetNextOrder(workId))
            .ReturnsAsync(order);

        return this;
    }
}
