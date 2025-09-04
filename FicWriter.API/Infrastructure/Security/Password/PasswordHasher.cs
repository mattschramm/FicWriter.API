namespace FicWriter.API.Infrastructure.Security.Password;

public class PasswordHasher : IPasswordHasher
{
    public string Hash(string password) => BCryptNet.EnhancedHashPassword(inputKey: password, workFactor: 13);
    public bool Verify(string password, string hashedPassword) => BCryptNet.EnhancedVerify(password, hashedPassword);
}
