using System.Security.Claims;

namespace FicWriter.API.Infrastructure.Services;

public class UserAccessor(IHttpContextAccessor acessor) : IUserAccessor
{
    private readonly IHttpContextAccessor _acessor = acessor;

    public ClaimsPrincipal User => _acessor.HttpContext!.User;
}
