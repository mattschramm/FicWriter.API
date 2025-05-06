using FicWriter.API.Models;

namespace FicWriter.API.Infrastructure.Data.Repositories.Tokens;

public interface ITokenRepository
{
    Task<RefreshToken?> Get(string token);
    void Update(RefreshToken token);
    Task Add(RefreshToken token);
    Task Delete(Guid userId);
}
