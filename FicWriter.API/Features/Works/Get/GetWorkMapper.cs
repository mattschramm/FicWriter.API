using FicWriter.API.Models;
using FicWriter.API.Shared.Mapper;
using Sqids;

namespace FicWriter.API.Features.Works.Get;

public class GetWorkMapper(SqidsEncoder<long> sqidsEncoder) : IFeatureMapper
{
    private readonly SqidsEncoder<long> _sqidsEncoder = sqidsEncoder;

    public GetWorkResponse ToResponse(Work work)
    {
        return new GetWorkResponse(
            WorkId: _sqidsEncoder.Encode(work.Id),
            Title: work.Title,
            Description: work.Description);
    }
}
