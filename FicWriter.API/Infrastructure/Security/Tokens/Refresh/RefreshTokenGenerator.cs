using FicWriter.API.Models;
using System.Security.Cryptography;

namespace FicWriter.API.Infrastructure.Security.Tokens.Refresh;

public class RefreshTokenGenerator : IRefreshTokenGenerator
{
    private const int TokenLength = 64;

    public string Generate() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(TokenLength));

}
