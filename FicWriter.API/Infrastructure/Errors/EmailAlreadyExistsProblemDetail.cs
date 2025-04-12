using Microsoft.AspNetCore.Mvc;

namespace FicWriter.API.Infrastructure.Errors;

public class EmailAlreadyExistsProblemDetail : ProblemDetails
{
    public EmailAlreadyExistsProblemDetail(string detail)
    {
        Title = "Email already exists";
        Status = StatusCodes.Status409Conflict;
        Detail = detail;
        Instance = $"/user";
    }
}
