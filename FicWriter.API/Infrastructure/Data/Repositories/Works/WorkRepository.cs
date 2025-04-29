using FicWriter.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FicWriter.API.Infrastructure.Data.Repositories.Works;

public class WorkRepository(FicWriterDbContext dbContext) : IWorkWriteOnly, IWorkReadOnly
{
    private readonly FicWriterDbContext _dbContext = dbContext;

    public async Task Create(Work work)
    {
        await _dbContext.Works.AddAsync(work);
    }

    public async Task<Work?> GetById(User user, long id) => await _dbContext.Works
        .AsNoTracking()
        .Include(w => w.Drafts)
        .FirstOrDefaultAsync(w => w.Id == id && w.UserId == user.Id && w.IsActive && !w.IsArchived);
}
