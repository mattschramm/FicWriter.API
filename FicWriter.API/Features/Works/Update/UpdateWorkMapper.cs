using FicWriter.API.Models;
using FicWriter.API.Shared.Mapper;

namespace FicWriter.API.Features.Works.Update;

public class UpdateWorkMapper : IFeatureMapper
{
    public void ToUpdatedWork(Work work, UpdateWorkCommand request)
    {
        work.Title = request.Title;
        work.Description = request.Description;
        work.UpdatedAt = DateTime.UtcNow;

        work.Genres.Clear();
        work.Tags.Clear();

        work.Genres = request.Genres.Select(g => new Genre
        {
            GenreType = g,
            WorkId = work.Id
        }).ToList();

        work.Tags = request.Tags.Select(t => new Tag
        {
            Content = t,
            WorkId = work.Id
        }).ToList();
    }
}
