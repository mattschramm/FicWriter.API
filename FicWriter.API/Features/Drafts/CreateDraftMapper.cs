using FicWriter.API.Models;
using FicWriter.API.Shared.Mapper;

namespace FicWriter.API.Features.Drafts;

public class CreateDraftMapper : IFeatureMapper
{
    public Draft ToDraft(CreateDraftCommand command) =>
        new()
        {
            Title = command.Title,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Order = command.Order,
            WorkId = command.WorkId
        };

    public CreateDraftResponse ToResponse(Draft draft) =>
        new(draft.Id, draft.Title, draft.CreatedAt, draft.UpdatedAt, draft.Order);
}
