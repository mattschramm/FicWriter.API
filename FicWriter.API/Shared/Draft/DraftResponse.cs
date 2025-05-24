namespace FicWriter.API.Shared.Draft;

public record DraftResponse(long Id, string Title, DateTime CreatedAt, DateTime UpdatedAt, uint Order);
