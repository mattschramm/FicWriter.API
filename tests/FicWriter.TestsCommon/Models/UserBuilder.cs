using Bogus;
using CommonTestUtils.Services;
using FicWriter.API.Models;

namespace CommonTestUtils.Models;

public static class UserBuilder
{
    public static (User user, string password) Build()
    {
        var passwordHasher = PasswordHasherBuilder.Build();

        var password = new Faker().Internet.Password();

        var user = new Faker<User>()
            .RuleFor(x => x.Id, () => 1)
            .RuleFor(x => x.Name, f => f.Person.FirstName)
            .RuleFor(x => x.Email, (f, x) => f.Internet.Email(x.Name))
            .RuleFor(x => x.Password, passwordHasher.Hash(password));

        return (user, password);
    }
}
