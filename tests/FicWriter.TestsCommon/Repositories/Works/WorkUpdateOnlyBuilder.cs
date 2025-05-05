using FicWriter.API.Infrastructure.Data.Repositories.Works;
using FicWriter.API.Models;
using Moq;

namespace CommonTestUtils.Repositories.Works;

public class WorkUpdateOnlyBuilder
{
    private readonly Mock<IWorkUpdateOnly> _workUpdateOnly;

    public WorkUpdateOnlyBuilder()
    {
        _workUpdateOnly = new Mock<IWorkUpdateOnly>();
    }

    public WorkUpdateOnlyBuilder GetWorkByIdWithTracking(User user, Work work)
    {
        _workUpdateOnly.Setup(x => x.GetWorkByIdWithTracking(user, work.Id))
            .ReturnsAsync(work);
        return this;
    }

    public IWorkUpdateOnly Build() => _workUpdateOnly.Object;
}
