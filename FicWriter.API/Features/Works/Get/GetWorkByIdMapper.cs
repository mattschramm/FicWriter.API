using FicWriter.API.Models;
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
            Drafts: work.Drafts,
            CreatedAt: work.CreatedAt,
            UpdatedAt: work.UpdatedAt,
            Tags: work.Tags,
            Genres: work.Genres);
    }
}
