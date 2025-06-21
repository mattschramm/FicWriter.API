using ErrorOr;

namespace FicWriter.API.Infrastructure.Errors;

public static class WorkErrors
{
    public static Error WorkNotFound() =>
        Error.NotFound(
            code: "Work.NotFound",
            description: "Work not found");

    public static Error InvalidWorkId() =>
        Error.Validation(
            code: "Work.InvalidId",
            description: "Invalid work ID provided");
}
