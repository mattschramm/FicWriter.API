namespace FicWriter.API.Infrastructure.Data.Repositories.Users;

public interface IUserReadOnly
{
    Task<bool> ExistsWithEmail(string email);
}