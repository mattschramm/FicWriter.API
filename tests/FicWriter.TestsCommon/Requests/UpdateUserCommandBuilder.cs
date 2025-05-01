using Bogus;
using FicWriter.API.Features.Users.Update;

namespace CommonTestUtils.Requests;

public static class UpdateUserCommandBuilder
{
    public static UpdateUserCommand Build()
    {
        return new Faker<UpdateUserCommand>()
            .CustomInstantiator(f => new UpdateUserCommand(
                f.Person.FirstName,
                f.Internet.Email(firstName: f.Person.FirstName)));
    }
}
