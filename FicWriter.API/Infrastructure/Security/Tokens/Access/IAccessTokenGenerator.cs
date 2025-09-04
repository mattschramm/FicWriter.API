namespace FicWriter.API.Infrastructure.Security.Tokens.Access;

public interface IAccessTokenGenerator
{
    string Generate(Guid guid);
}