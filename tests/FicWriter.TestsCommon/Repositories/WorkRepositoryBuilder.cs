using FicWriter.API.Infrastructure.Data.Repositories.Works;
using FicWriter.API.Models;
using Moq;

namespace CommonTestUtils.Repositories;

public class WorkRepositoryBuilder
{
    private readonly Mock<IWorkRepository> _mockRepository;

    public WorkRepositoryBuilder()
    {
        _mockRepository = new Mock<IWorkRepository>();
    }

    public WorkRepositoryBuilder GetById(Work work, User user)
    {
        _mockRepository
            .Setup(w => w.GetById(user, work.Id))
            .ReturnsAsync(work);
        return this;
    }

    public WorkRepositoryBuilder GetWorkByIdWithTracking(Work work, User user)
    {
        _mockRepository
            .Setup(w => w.GetWorkByIdWithTracking(user, work.Id))
            .ReturnsAsync(work);
        return this;
    }

    public IWorkRepository Build() => _mockRepository.Object;
}
