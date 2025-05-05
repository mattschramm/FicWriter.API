using FicWriter.API.Models;

namespace FicWriter.API.Infrastructure.Data.Repositories.Works;

public interface IWorkUpdateOnly
{
    void Update(Work work);
    Task<Work?> GetWorkByIdWithTracking(User user, long id);
}
