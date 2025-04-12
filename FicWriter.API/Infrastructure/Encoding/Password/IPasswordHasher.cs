namespace FicWriter.API.Infrastructure.Encoding.Password;
public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string password, string hashedPassword);
}