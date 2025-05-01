using Bogus;
using FicWriter.API.Features.Users.Login;

namespace CommonTestUtils.Requests;

public static class LoginCommandBuilder
{
    public static LoginCommand Build()
    {
        return new Faker<LoginCommand>()
            .RuleFor(x => x.Email, f => f.Internet.Email())
            .RuleFor(x => x.Password, f => f.Internet.Password());
    }

    public static LoginCommand Build(string email, string password) => new(email, password);
}
