using System.Security.Claims;

namespace FicWriter.API.Infrastructure.Services;

public interface IUserAccessor
{
    ClaimsPrincipal User { get; }
}