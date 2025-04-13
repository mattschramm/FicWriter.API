using FicWriter.API.Infrastructure.Security.Password;

namespace CommonTestUtils.Services;

public static class PasswordHasherBuilder
{
    public static PasswordHasher Build()
    {
        var passwordHasher = new PasswordHasher();
        return passwordHasher;
    }
}
