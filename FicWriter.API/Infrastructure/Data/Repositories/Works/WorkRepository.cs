using FicWriter.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FicWriter.API.Infrastructure.Data.Repositories.Works;

public class WorkRepository(FicWriterDbContext dbContext) : IWorkWriteOnly, IWorkReadOnly
{
    private readonly FicWriterDbContext _dbContext = dbContext;

    public async Task Create(Work work)
    {
        await _dbContext.Works.AddAsync(work);
    }

    public async Task Delete(long id)
    {
        var work = await _dbContext.Works.FindAsync(id);
        
        if (work is not null)
        {
            work.IsActive = false;
        }
    }

    public Task<bool> Exists(User user, long id) => _dbContext.Works
        .AsNoTracking()
        .AnyAsync(w => w.Id == id && w.UserId == user.Id && w.IsActive);

    public async Task<Work?> GetById(User user, long id) => await _dbContext.Works
        .AsNoTracking()
        .Include(w => w.Drafts)
        .FirstOrDefaultAsync(w => w.Id == id && w.UserId == user.Id && w.IsActive && !w.IsArchived);
}
