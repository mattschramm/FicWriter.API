using FicWriter.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FicWriter.API.Infrastructure.Data.Repositories.Tokens;

public class TokenRepository(FicWriterDbContext dbContext) : ITokenWriteOnly, ITokenReadOnly, ITokenUpdateOnly
{
    private readonly FicWriterDbContext _dbContext = dbContext;

    public async Task Add(RefreshToken token) => await _dbContext.RefreshTokens.AddAsync(token);

    public async Task Delete(Guid userId) => await _dbContext.RefreshTokens
        .Where(t => t.User.UserIdentifier == userId)
        .ExecuteDeleteAsync();

    public Task<RefreshToken?> Get(string token)
    {
        return _dbContext.RefreshTokens
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.Token == token);
    }

    public void Update(RefreshToken token) => _dbContext.RefreshTokens.Update(token);
}
