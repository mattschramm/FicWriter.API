namespace FicWriter.API.Infrastructure.Data;

public interface IUnitOfWork
{
    Task Commit();
}