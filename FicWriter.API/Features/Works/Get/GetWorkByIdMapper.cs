using FicWriter.API.Models;
using FicWriter.API.Shared.Draft;
using FicWriter.API.Shared.Mapper;
using Sqids;

namespace FicWriter.API.Features.Works.Get;

public class GetWorkByIdMapper(SqidsEncoder<long> sqidsEncoder) : IFeatureMapper
{
    private readonly SqidsEncoder<long> _sqidsEncoder = sqidsEncoder;

    public GetWorkByIdResponse ToResponse(Work work)
    {
        return new GetWorkByIdResponse(
            Id: _sqidsEncoder.Encode(work.Id),
            Title: work.Title,
            Description: work.Description,
            Drafts: MapDrafts(work.Drafts),
            CreatedAt: work.CreatedAt,
            UpdatedAt: work.UpdatedAt,
            Tags: work.Tags.Select(t => t.Content).ToList(),
            Genres: work.Genres.Select(g => g.GenreType).ToList());
    }

    private static List<DraftResponse> MapDrafts(List<Draft> drafts)
    {
        var draftsResponse = new List<DraftResponse>();

        foreach (var draft in drafts)
        {
            draftsResponse.Add(new DraftResponse(
                Id: draft.Id,
                Title: draft.Title,
                CreatedAt: draft.CreatedAt,
                UpdatedAt: draft.UpdatedAt,
                Order: draft.Order));
        }

        return draftsResponse;
    }
}
