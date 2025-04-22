using FicWriter.API.Models;

namespace FicWriter.API.Infrastructure.Data.Repositories.Users;

public interface IUserReadOnly
{
    Task<bool> ExistsWithEmail(string email);
    Task<User?> GetByEmail(string email);
    Task<User?> GetById(long id);
    Task<User?> GetByUserIdentifier(Guid userIdentifier);
    Task<long> GetIdByUserIdentifier(Guid userIdentifier);
}