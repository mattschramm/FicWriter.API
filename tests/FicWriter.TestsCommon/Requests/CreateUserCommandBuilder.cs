using Bogus;
using FicWriter.API.Features.Users.Create;

namespace CommonTestUtils.Requests;

public static class CreateUserCommandBuilder
{
    public static CreateUserCommand Build(int passwordLength = 10)
    {

        return new Faker<CreateUserCommand>()
        .CustomInstantiator(f => new CreateUserCommand(
            f.Person.FirstName,
            f.Internet.Email(firstName: f.Person.FirstName),
            f.Internet.Password(passwordLength))).Generate();
    }
}
