using FicWriter.API.Enums;

namespace FicWriter.API.Models;

public class Genre
{
    public Genres GenreType { get; set; }
    public long WorkId { get; set; }
}
