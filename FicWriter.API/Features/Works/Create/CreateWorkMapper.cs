using FicWriter.API.Models;
using FicWriter.API.Shared.Mapper;
using Sqids;

namespace FicWriter.API.Features.Works.Create;

public class CreateWorkMapper(SqidsEncoder<long> encoder) : IFeatureMapper
{
    private readonly SqidsEncoder<long> _encoder = encoder;

    public Work ToWork(CreateWorkCommand command, long userId) =>
        new()
        {
            Title = command.Title,
            Description = command.Description,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            UserId = userId,
        };

    public CreateWorkResponse ToResponse(Work work)
    {
        var encryptedId = _encoder.Encode(work.Id);
        return new(encryptedId, work.Title);
    }
}
