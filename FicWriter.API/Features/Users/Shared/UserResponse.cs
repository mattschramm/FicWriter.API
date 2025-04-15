using FicWriter.API.Models;

namespace FicWriter.API.Features.Users.Shared;

public record Tokens(string AccessToken, string RefreshToken);
public record UserResponse(string Name, Tokens AccessToken);
