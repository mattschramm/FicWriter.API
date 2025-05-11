using FicWriter.API.Features.Works.Dashboard;
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

    // technically we are basically testing nothing from the original function due its nature,
    // but the filters here are implemented in the same way and therefore is testable.
    // The integration tests will tell us more.
    public WorkRepositoryBuilder GetDashboard(List<Work> works, User user, GetDashboardCommand command)
    {
        var filteredWorks = new List<Work>(works);
        
        if (command.Title is not null)
        {
            filteredWorks = filteredWorks
                .Where(w => w.Title.Contains(command.Title, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
        
        if (command.Genres is not null && command.Genres.Length > 0)
        {
            filteredWorks = filteredWorks
                .Where(w => w.Genres.Any(g => command.Genres.Contains(g.GenreType)))
                .ToList();
        }

        if (command.Tags is not null && command.Tags.Length > 0)
        {
            filteredWorks = filteredWorks
                .Where(w => w.Tags.Any(t => command.Tags.Contains(t.Content)))
                .ToList();
        }

        _mockRepository
            .Setup(w => w.GetDashboard(user, command))
            .ReturnsAsync(filteredWorks);
        return this;
    }

    public IWorkRepository Build() => _mockRepository.Object;
}
