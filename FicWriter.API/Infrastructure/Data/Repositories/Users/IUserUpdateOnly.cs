using FicWriter.API.Models;

namespace FicWriter.API.Infrastructure.Data.Repositories.Users;

public interface IUserUpdateOnly
{
    void Update(User user);
    Task<User?> GetUserByIdWithTracking(long id);
}
