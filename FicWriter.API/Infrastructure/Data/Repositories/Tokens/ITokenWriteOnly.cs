using FicWriter.API.Models;

namespace FicWriter.API.Infrastructure.Data.Repositories.Tokens;

public interface ITokenWriteOnly
{
    Task Add(RefreshToken token);
}