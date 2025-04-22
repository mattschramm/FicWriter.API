using FicWriter.API.Models;

namespace FicWriter.API.Infrastructure.Data.Repositories.Tokens;

public interface ITokenUpdateOnly
{
    void Update(RefreshToken token);
}
