using FicWriter.API.Models;
using FicWriter.API.Shared.Mapper;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Sqids;

namespace FicWriter.API.Features.Works.Get;

public class GetWorkMapper : IFeatureMapper
{
    private readonly SqidsEncoder<long> _sqidsEncoder;

    public GetWorkMapper(SqidsEncoder<long> sqidsEncoder)
    {
        _sqidsEncoder = sqidsEncoder;
    }

    public GetWorkResponse ToResponse(GetWorkCommand command, GetWorkResponse response)
    {
        return new GetWorkResponse(
            WorkId: _sqidsEncoder.Encode(command.WorkId),
            Title: response.Title,
            Description: response.Description);
    }
}
