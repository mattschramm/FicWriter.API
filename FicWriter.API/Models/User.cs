using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FicWriter.API.Models;

public class User
{
    public long Id { get; set; }
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public Guid UserIdentifier { get; set; }
}
