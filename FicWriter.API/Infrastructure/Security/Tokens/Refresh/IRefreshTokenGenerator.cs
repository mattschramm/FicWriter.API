using FicWriter.API.Models;

namespace FicWriter.API.Infrastructure.Security.Tokens.Refresh;

public interface IRefreshTokenGenerator
{
    string Generate();
}