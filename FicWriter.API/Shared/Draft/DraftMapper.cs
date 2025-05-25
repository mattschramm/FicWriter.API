using FicWriter.API.Shared.Mapper;

namespace FicWriter.API.Shared.Draft;

public class DraftMapper : IFeatureMapper
{
    public DraftResponse ToResponse(Models.Draft draft)
    {
        return new DraftResponse(
            Id: draft.Id,
            Title: draft.Title,
            CreatedAt: draft.CreatedAt,
            UpdatedAt: draft.UpdatedAt,
            Order: draft.Order);
    }
}
