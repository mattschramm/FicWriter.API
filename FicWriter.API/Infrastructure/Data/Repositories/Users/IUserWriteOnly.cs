using FicWriter.API.Models;

namespace FicWriter.API.Infrastructure.Data.Repositories.Users;

public interface IUserWriteOnly
{
    Task Add(User user);
}
