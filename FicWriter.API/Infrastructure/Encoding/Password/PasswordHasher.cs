namespace FicWriter.API.Infrastructure.Encoding.Password;

public class PasswordHasher : IPasswordHasher
{
    public string Hash(string password) => BCryptNet.EnhancedHashPassword(password, workFactor: 15);
    public bool Verify(string password, string hashedPassword) => BCryptNet.EnhancedVerify(password, hashedPassword);
}
