using Bogus;
using FicWriter.API.Enums;
using FicWriter.API.Features.Works.Update;

namespace CommonTestUtils.Requests;

public static class UpdateWorkCommandBuilder
{
    public static UpdateWorkCommand Build (long id)
    {
        return new Faker<UpdateWorkCommand>()
            .CustomInstantiator(f => new UpdateWorkCommand(
                id,
                f.Lorem.Sentence(1),
                f.Lorem.Paragraph(1),
                f.Make(1, f.PickRandom<Genres>).ToList(),
                f.Make(1, f.Lorem.Word).ToList()
                ))
            .Generate();
    }
}
