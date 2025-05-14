using FicWriter.API.Models;

namespace FicWriter.API.Infrastructure.Data.Repositories.Drafts;

public class DraftRepository(FicWriterDbContext dbContext) : IDraftRepository
{
    private readonly FicWriterDbContext _dbContext = dbContext;

    public async Task Create(Draft draft) => await _dbContext.Drafts.AddAsync(draft);
}
