using FicWriter.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace FicWriter.API.Infrastructure.Data.Repositories.Users;

public class UserRepository(FicWriterDbContext dbContext) : IUserReadOnly, IUserWriteOnly
{
    private readonly FicWriterDbContext _dbContext = dbContext;

    public async Task<bool> ExistsWithEmail(string email)
    {
        return await _dbContext.Users
                .AsNoTracking()
                .AnyAsync(u => u.Email == email);
    }

    public async Task Add(User user) => await _dbContext.Users.AddAsync(user);

    public async Task<User?> GetByEmail(string email)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetById(long id)
    {
        return await _dbContext.Users
        .AsNoTracking()
        .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetByUserIdentifier(Guid userIdentifier)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.UserIdentifier == userIdentifier);
    }

    public Task<long> GetIdByUserIdentifier(Guid userIdentifier) =>
        _dbContext.Users
            .AsNoTracking()
            .Where(u => u.UserIdentifier == userIdentifier)
            .Select(u => u.Id)
            .FirstOrDefaultAsync();
}
