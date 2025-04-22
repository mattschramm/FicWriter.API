using Bogus;
using FicWriter.API.Features.Users.Login;

namespace CommonTestUtils.Requests;

public static class LoginRequestBuilder
{
    public static LoginRequest Build()
    {
        return new Faker<LoginRequest>()
            .RuleFor(x => x.Email, f => f.Internet.Email())
            .RuleFor(x => x.Password, f => f.Internet.Password());
    }

    public static LoginRequest Build(string email, string password) => new(email, password);
}
