using FicWriter.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FicWriter.API.Infrastructure.Data.Repositories.Drafts;

public class DraftRepository(FicWriterDbContext dbContext) : IDraftRepository
{
    private readonly FicWriterDbContext _dbContext = dbContext;

    public async Task Create(Draft draft) => await _dbContext.Drafts.AddAsync(draft);

    public async Task<Draft?> GetDraftById(long workId, long draftId) => await _dbContext.Drafts
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.WorkId == workId && d.Id == draftId);

    public async Task<uint> GetNextOrder(long workId)
    {
        var maxOrder = await GetDraftsList(workId)
            .MaxAsync(d => (uint?)d.Order) ?? 0;

        return maxOrder + 1;
    }

    public async Task<List<Draft>> GetDrafts(long workId) => await GetDraftsList(workId)
            .OrderByDescending(d => d.Order)
            .ToListAsync();

    public void Update(Draft draft) => _dbContext.Drafts.Update(draft);
    
    public Task<Draft?> GetDraftByIdWithTracking(long draftId) => _dbContext.Drafts
            .FirstOrDefaultAsync(d => d.Id == draftId);

    public async Task<bool> Delete(long draftId)
    {
        var draft = await _dbContext.Drafts
            .FirstOrDefaultAsync(d => d.Id == draftId);

        if (draft is null)
        {
            return false;
        }

        await _dbContext.Drafts
            .Where(d => d.WorkId == draft.WorkId && d.Order > draft.Order)
            .ExecuteUpdateAsync(d => d.SetProperty(d => d.Order, d => d.Order - 1));

        _dbContext.Drafts.Remove(draft);

        return true;
    }

    public async Task<bool> Exists(long draftId) => await _dbContext.Drafts
        .AsNoTracking()
        .AnyAsync(d => d.Id == draftId);

    private IQueryable<Draft> GetDraftsList(long workId) => _dbContext.Drafts
            .AsNoTracking()
            .Where(d => d.WorkId == workId);
}
