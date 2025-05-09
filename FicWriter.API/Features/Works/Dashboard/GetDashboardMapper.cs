using FicWriter.API.Models;
using FicWriter.API.Shared.Mapper;
using FicWriter.API.Shared.Tag;
using Sqids;
using System.Text;

namespace FicWriter.API.Features.Works.Dashboard;

public class GetDashboardMapper(SqidsEncoder<long> sqidsEncoder) : IFeatureMapper
{
    private readonly SqidsEncoder<long> _sqidsEncoder = sqidsEncoder;

    public GetDashboardResponse ToResponse(List<Work> works)
    {
        var workResponses = works.Select(work => new GetWorksResponse(
            Id: _sqidsEncoder.Encode(work.Id),
            Title: work.Title,
            ShortDescription: GetShortDescription(work.Description),
            UpdatedAt: work.UpdatedAt,
            Genres: work.Genres.Select(g => g.GenreType).ToList(),
            Tags: work.Tags.Select(tag => new TagResponse(tag.WorkId, tag.Content)).ToList()
        )).ToList();

        return new GetDashboardResponse(works.Count, workResponses);
    }

    private static string GetShortDescription(string description)
    {
        var sb = new StringBuilder();

        if (description.Length > 100)
        {
            sb.Append(description[..100]);
            sb.Append("...");
        }
        else
        {
            sb.Append(description);
        }

        return sb.ToString();
    }
}
