using FicWriter.API.Models;

namespace FicWriter.API.Infrastructure.Services;

public interface ICurrentUser
{
    Task<User> GetCurrentUser();
}