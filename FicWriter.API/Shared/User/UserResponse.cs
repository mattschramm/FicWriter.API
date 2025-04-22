namespace FicWriter.API.Shared.User;

public record Tokens(string AccessToken, string RefreshToken);
public record UserResponse(string Name, Tokens Tokens);
