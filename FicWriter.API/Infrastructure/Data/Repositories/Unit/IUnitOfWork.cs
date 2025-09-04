namespace FicWriter.API.Infrastructure.Data.Repositories.Unit;

public interface IUnitOfWork
{
    Task Commit();
}