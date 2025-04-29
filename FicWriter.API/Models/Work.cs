namespace FicWriter.API.Models;

public class Work
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsArchived { get; set; } = false;
    public List<Draft> Drafts { get; set; } = [];
    public long UserId { get; set; }
    public User User { get; set; } = default!;
}
