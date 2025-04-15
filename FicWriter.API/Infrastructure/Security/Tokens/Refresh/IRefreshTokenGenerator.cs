using FicWriter.API.Models;

namespace FicWriter.API.Infrastructure.Security.Tokens.Refresh;

public interface IRefreshTokenGenerator
{
    RefreshToken Generate(long userId);
}