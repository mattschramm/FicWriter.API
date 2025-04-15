using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace FicWriter.API.Infrastructure.Errors;

public static class ErrorsExtensions
{
    private static readonly Dictionary<string, string> _customErrors = new()
    {
        { "User.EmailAlreadyExists", "Email already exists." },
        { "User.InvalidCredentials", "Invalid credentials." },
    };

    public static IResult ToProblem(this List<Error> errors)
    {
        if (errors.Count == 0)
        {
            return Results.Problem(detail:"An Unknown problem has occurred", statusCode: 500);
        }

        var firstError = errors.First();

        var statusCode = firstError.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError
        };


        var title = _customErrors.TryGetValue(firstError.Code, out var customTitle)
            ? customTitle
            : firstError.Description;

        var problemDetails = new ProblemDetails
        {
            Title = title,
            Detail = firstError.Description,
            Status = statusCode,
            Extensions =
            {
                ["errors"] = errors.Select(e => e.Description).ToList()
            }
        };

        return Results.Problem(problemDetails);
    }

    public static IResult ToProblem<T>(this ErrorOr<T> errorOr) => errorOr.Errors.ToProblem();
}
