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
                f.Lorem.Paragraph(1)))
            .Generate();
    }
}
