using FicWriter.API.Infrastructure.Data.Repositories.Works;
using Moq;
using FicWriter.API.Models;

namespace CommonTestUtils.Repositories.Works;

public class WorkReadOnlyBuilder
{
    private readonly Mock<IWorkReadOnly> _workReadOnlyMock;

    public WorkReadOnlyBuilder()
    {
        _workReadOnlyMock = new Mock<IWorkReadOnly>();
    }

    public WorkReadOnlyBuilder GetById(Work work, User user)
    {
        _workReadOnlyMock
            .Setup(w => w.GetById(user, work.Id))
            .ReturnsAsync(work);
        return this;
    }

    public IWorkReadOnly Build() => _workReadOnlyMock.Object;
}
