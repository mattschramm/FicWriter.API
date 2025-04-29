using FicWriter.API.Models;

namespace FicWriter.API.Infrastructure.Data.Repositories.Works;

public interface IWorkWriteOnly
{
    Task Create(Work work);
}
