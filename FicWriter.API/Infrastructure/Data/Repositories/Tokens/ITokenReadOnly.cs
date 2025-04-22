using FicWriter.API.Models;

namespace FicWriter.API.Infrastructure.Data.Repositories.Tokens;

public interface ITokenReadOnly
{
    Task<RefreshToken?> Get(string token);
}