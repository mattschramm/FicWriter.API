using FicWriter.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FicWriter.API.Infrastructure.Data.Repositories.Drafts;

public class DraftRepository(FicWriterDbContext dbContext) : IDraftRepository
{
    private readonly FicWriterDbContext _dbContext = dbContext;

    public async Task Create(Draft draft) => await _dbContext.Drafts.AddAsync(draft);
    
    public async Task<Draft?> GetDraftById(long workId, long draftId) =>
        await _dbContext.Drafts
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.WorkId == workId && d.Id == draftId);

    public async Task<uint> GetNextOrder(long workId)
    {
        var maxOrder = await _dbContext.Drafts
            .AsNoTracking()
            .Where(d => d.WorkId == workId)
            .MaxAsync(d => (uint?)d.Order) ?? 0;

        return maxOrder + 1;
    }
}
