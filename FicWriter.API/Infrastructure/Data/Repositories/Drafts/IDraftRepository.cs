using FicWriter.API.Models;

namespace FicWriter.API.Infrastructure.Data.Repositories.Drafts;

public interface IDraftRepository
{
    Task Create(Draft draft);
    Task<Draft?> GetDraftById(long workId, long draftId);
    Task<Draft?> GetDraftByIdWithTracking(long workId, long draftId);
    Task<List<Draft>> GetDrafts(long workId);
    Task<uint> GetNextOrder(long workId);
    void Update(Draft draft);
}
