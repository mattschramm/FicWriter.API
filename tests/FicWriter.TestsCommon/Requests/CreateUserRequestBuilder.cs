using Bogus;
using FicWriter.API.Features.Users.Create;

namespace CommonTestUtils.Requests;

public static class CreateUserRequestBuilder
{
    public static CreateUserCommand Build(int passwordLength = 10)
    {
        var faker = new Faker<CreateUserCommand>()
        .CustomInstantiator(f => new CreateUserCommand(
            f.Person.FirstName,
            f.Internet.Email(firstName: f.Person.FirstName),
            f.Internet.Password(passwordLength)));

        return faker.Generate();
    }
}
