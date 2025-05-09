using FicWriter.API.Enums;
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
            Genres = ToGenres(command.Genres),
            Tags = ToTags(command.Tags),
        };

    public CreateWorkResponse ToResponse(Work work)
    {
        var encryptedId = _encoder.Encode(work.Id);
        return new(encryptedId, work.Title);
    }

    private static List<Genre> ToGenres(List<Genres> genres) =>
        genres.Select(g => new Genre
        {
            GenreType = g,
        }).ToList();

    private static List<Tag> ToTags(List<string> tags) =>
        tags.Select(tag => new Tag
        {
            Content = tag
        }).ToList();
}
