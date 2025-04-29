using FicWriter.API.Models;

namespace FicWriter.API.Infrastructure.Data.Repositories.Works;

public class WorkRepository(FicWriterDbContext dbContext) : IWorkWriteOnly
{
    private readonly FicWriterDbContext _dbContext = dbContext;

    public async Task Create(Work work)
    {
        await _dbContext.Works.AddAsync(work);
    }
}
