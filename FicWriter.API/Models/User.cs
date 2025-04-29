namespace FicWriter.API.Models;

public class User
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid UserIdentifier { get; set; }
    public bool IsActive { get; set; } = true;
    public List<Work> Works { get; set; } = [];
}
