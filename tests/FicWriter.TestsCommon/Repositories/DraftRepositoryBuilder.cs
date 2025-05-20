using FicWriter.API.Infrastructure.Data.Repositories.Drafts;
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
}
