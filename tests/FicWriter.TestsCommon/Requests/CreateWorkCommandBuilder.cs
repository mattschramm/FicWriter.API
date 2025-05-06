using Bogus;
using FicWriter.API.Features.Works.Create;

namespace CommonTestUtils.Requests;

public static class CreateWorkCommandBuilder
{
    public static CreateWorkCommand Build()
    {
        return new Faker<CreateWorkCommand>()
            .CustomInstantiator(f => new CreateWorkCommand(
                f.Lorem.Sentence(3),
                f.Lorem.Paragraph(1),
                
                f.Make(1, () => new FicWriter.API.Models.Genre
                {
                    GenreType = f.PickRandom<FicWriter.API.Enums.Genres>(),
                    WorkId = 1
                }).ToList(),

                f.Make(2, () => new FicWriter.API.Models.Tag
                {
                    Content = f.Lorem.Word(),
                    WorkId = 1
                }).ToList()))
            .Generate();
    }
}
