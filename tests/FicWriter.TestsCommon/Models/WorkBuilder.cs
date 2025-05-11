using Bogus;
using FicWriter.API.Enums;
using FicWriter.API.Models;

namespace CommonTestUtils.Models;

public static class WorkBuilder
{
    public static Work Build(User user, long id = 1)
    {
        return new Faker<Work>()
            .RuleFor(w => w.Id, f => id)
            .RuleFor(w => w.Title, f => f.Lorem.Sentence(1))
            .RuleFor(w => w.Description, f => f.Lorem.Paragraph(1))
            .RuleFor(w => w.UserId, f => user.Id)
            .RuleFor(w => w.User, f => user)
            .RuleFor(w => w.Genres, f => f.Make(1, () => new Genre
            {
                GenreType = f.PickRandom<Genres>(),
                WorkId = id
            }))
            .RuleFor(w => w.Tags, f => f.Make(1, () => new Tag
            {
                Content = f.Lorem.Word(),
                WorkId = id
            }));
    }

    public static List<Work> Build(User user, int quantity, long id = 1)
    {
        var works = new List<Work>();

        for (var i = 0; i < quantity; i++)
        {
            works.Add(Build(user, id));
        }

        return works;
    }
}
