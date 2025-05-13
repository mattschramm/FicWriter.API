using FicWriter.API.Features.Works.Dashboard;
using FicWriter.API.Models;

namespace FicWriter.API.Infrastructure.Data.Repositories.Works;

public interface IWorkRepository
{
    Task<Work?> GetById(User user, long id);
    Task<bool> Exists(User user, long id);
    void Update(Work work);
    Task<Work?> GetWorkByIdWithTracking(User user, long id, bool includeArchived = false);
    Task<List<Work>> GetArchivedWorks(User user, long id, bool withTracking = false);
    Task<List<Work>> GetAllWorks(User user, long id, bool withTracking = false);
    Task Create(Work work);
    Task Delete(long id);
    Task<List<Work>> GetDashboard(User user, GetDashboardCommand command);
}
