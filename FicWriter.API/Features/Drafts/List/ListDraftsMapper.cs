using FicWriter.API.Models;
using FicWriter.API.Shared.Draft;
using FicWriter.API.Shared.Mapper;

namespace FicWriter.API.Features.Drafts.List;

public class ListDraftsMapper : IFeatureMapper
{
    public ListDraftsResponse ToResponse(List<Draft> drafts)
    {
        var draftResponses = drafts.Select(draft => new DraftResponse(
            Id: draft.Id,
            Title: draft.Title,
            CreatedAt: draft.CreatedAt,
            UpdatedAt: draft.UpdatedAt,
            Order: draft.Order
        )).ToList();

        return new ListDraftsResponse(draftResponses, draftResponses.Count);
    }
}
