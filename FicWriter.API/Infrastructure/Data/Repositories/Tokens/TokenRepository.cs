using FicWriter.API.Models;

namespace FicWriter.API.Infrastructure.Data.Repositories.Tokens;

public class TokenRepository(FicWriterDbContext dbContext) : ITokenWriteOnly
{
    private readonly FicWriterDbContext _dbContext = dbContext;

    public async Task Add(RefreshToken token) => await _dbContext.RefreshTokens.AddAsync(token);
}
