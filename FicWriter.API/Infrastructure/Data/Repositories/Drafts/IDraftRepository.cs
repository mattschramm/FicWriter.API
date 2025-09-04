using FicWriter.API.Models;

namespace FicWriter.API.Infrastructure.Data.Repositories.Drafts;

public interface IDraftRepository
{
    Task Create(Draft draft);
    Task<bool> Delete(long draftId);
    Task<bool> Exists(long draftId);
    Task<Draft?> GetDraftById(long workId, long draftId);
    Task<Draft?> GetDraftByIdWithTracking(long draftId);
    Task<List<Draft>> GetDrafts(long workId);
    Task<uint> GetNextOrder(long workId);
    void Update(Draft draft);
}
