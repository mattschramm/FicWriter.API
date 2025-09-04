using Bogus;
using FicWriter.API.Features.Drafts.Create;

namespace CommonTestUtils.Requests;

public static class CreateDraftCommandBuilder
{
    public static CreateDraftCommand Build()
    {
        return new Faker<CreateDraftCommand>()
            .CustomInstantiator(f => new CreateDraftCommand(
                f.Lorem.Sentence(3),
                f.Random.UInt()
            ))
            .Generate();
    }
}
