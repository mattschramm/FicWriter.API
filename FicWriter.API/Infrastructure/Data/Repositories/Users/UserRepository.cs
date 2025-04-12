using Microsoft.EntityFrameworkCore;

namespace FicWriter.API.Infrastructure.Data.Repositories.Users;

public class UserRepository(FicWriterDbContext dbContext) : IUserReadOnly, IUserWriteOnly
{
    private readonly FicWriterDbContext _dbContext = dbContext;

    public async Task<bool> ExistsWithEmail(string email)
    {
        return await _dbContext.Users.AnyAsync(u => u.Email == email);
    }
    
    public async Task Create(Models.User user)
    {
        await _dbContext.Users.AddAsync(user);
    }
}
