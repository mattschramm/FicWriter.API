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
        var maxOrder = await GetDraftsList(workId)
            .MaxAsync(d => (uint?)d.Order) ?? 0;

        return maxOrder + 1;
    }

    public async Task<List<Draft>> GetDrafts(long workId) =>
        await GetDraftsList(workId)
        .OrderByDescending(d => d.Order)
            .ToListAsync();

    private IQueryable<Draft> GetDraftsList(long workId)
    {
        return _dbContext.Drafts
            .AsNoTracking()
            .Where(d => d.WorkId == workId);
    }
}
