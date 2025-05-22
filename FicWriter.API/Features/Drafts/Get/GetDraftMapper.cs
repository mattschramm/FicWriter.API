using FicWriter.API.Models;
using FicWriter.API.Shared.Mapper;

namespace FicWriter.API.Features.Drafts.Get;

public class GetDraftMapper : IFeatureMapper
{
    public GetDraftResponse ToResponse(Draft draft) =>
        new(draft.Id, draft.Title, draft.CreatedAt, draft.UpdatedAt, draft.Order);
}
