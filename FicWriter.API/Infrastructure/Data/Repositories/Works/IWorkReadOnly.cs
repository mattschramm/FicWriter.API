using FicWriter.API.Models;

namespace FicWriter.API.Infrastructure.Data.Repositories.Works;

public interface IWorkReadOnly
{
    Task<Work?> GetById(User user, long id);
}
