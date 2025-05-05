using ErrorOr;

namespace FicWriter.API.Infrastructure.Errors;

public static class WorkErrors
{
    public static Error WorkNotFound() =>
        Error.NotFound(
            code: "Work.NotFound",
            description: "Work not found");
}
