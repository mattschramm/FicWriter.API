namespace FicWriter.API.Features.Users.Common;

public record Tokens(string AccessToken, string RefreshToken);
public record UserResponse(string Name, Tokens Tokens);
