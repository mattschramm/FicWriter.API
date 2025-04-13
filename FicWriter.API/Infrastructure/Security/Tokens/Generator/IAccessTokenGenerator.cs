namespace FicWriter.API.Infrastructure.Security.Tokens.Generator;

public interface IAccessTokenGenerator
{
    string Generate(Guid guid);
}