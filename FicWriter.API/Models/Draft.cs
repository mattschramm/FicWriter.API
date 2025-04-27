namespace FicWriter.API.Models;

public class Draft
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int Order { get; set; }
    public long WorkId { get; set; }
    public Work Work { get; set; } = default!;
}
