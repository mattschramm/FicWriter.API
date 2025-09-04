namespace FicWriter.API.Models;

public class RefreshToken
{
    public Guid Id { get; set; }
    public required string Token { get; set; }
    public long UserId { get; set; }
    public DateTime ExpiresOnUtc { get; set; }
    public User User { get; set; } = default!;
}
