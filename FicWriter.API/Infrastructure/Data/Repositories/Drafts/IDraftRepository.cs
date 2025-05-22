using FicWriter.API.Models;

namespace FicWriter.API.Infrastructure.Data.Repositories.Drafts;

public interface IDraftRepository
{
    Task Create(Draft draft);
    Task<Draft?> GetDraftById(long workId, long draftId);
}
