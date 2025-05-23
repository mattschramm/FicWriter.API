using ErrorOr;

namespace FicWriter.API.Infrastructure.Errors;

public static class DraftErrors
{
    public static Error DraftNotFound() =>
        Error.NotFound(
            code: "Draft.NotFound",
            description: $"Draft not found");
}
