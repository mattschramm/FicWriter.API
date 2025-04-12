using Bogus;
using FicWriter.API.Features.Users.Create;

namespace CommonTestUtils.Requests;

public static class CreateUserRequestBuilder
{
    public static CreateUserRequest Build(int passwordLength = 10)
    {
        var faker = new Faker<CreateUserRequest>()
        .CustomInstantiator(f => new CreateUserRequest(
            f.Person.FirstName,
            f.Internet.Email(firstName: f.Person.FirstName),
            f.Internet.Password(passwordLength)));

        return faker.Generate();
    }
}
