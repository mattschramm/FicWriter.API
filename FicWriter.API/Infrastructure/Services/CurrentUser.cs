using FicWriter.API.Infrastructure.Data;
using FicWriter.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FicWriter.API.Infrastructure.Services;

public class CurrentUser(IHttpContextAccessor acessor, FicWriterDbContext dbContext) : ICurrentUser
{
    private readonly IHttpContextAccessor _acessor = acessor;
    private readonly FicWriterDbContext _dbContext = dbContext;

    public async Task<User> GetCurrentUser()
    {
        var requestId = Guid.Parse(_acessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        return await _dbContext.Users.FirstAsync(i => i.UserIdentifier == requestId);
    }
}
