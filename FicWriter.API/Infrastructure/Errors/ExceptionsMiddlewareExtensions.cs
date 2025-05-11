using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace FicWriter.API.Infrastructure.Errors;

// this is so cursed lol
public static partial class ExceptionsMiddlewareExtensions
{
    public static void UseBindingExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(exceptionHandlerApp =>
        {
            exceptionHandlerApp.Run(async context =>
            {
                var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
                
                var statusCode = StatusCodes.Status500InternalServerError;
                var title = "An unexpected error occurred";

                var errors = new Dictionary<string, string[]>();

                if (exception is BadHttpRequestException badRequest 
                    && badRequest.Message.Contains("Failed to bind parameter"))
                {
                    statusCode = badRequest.StatusCode;
                    title = "Invalid value in query request";

                    var typeName = ParameterBindingRegex().Match(badRequest.Message);

                    if (typeName is not null)
                    {
                        var parameterName = typeName.Groups[1].Value;
                        var errorMessage = $"Invalid value in query request for parameter {parameterName}";
                        errors.Add(parameterName, [errorMessage]);
                    }
                }

                var problemDetails = new ValidationProblemDetails
                {
                    Status = statusCode,
                    Title = title,
                    Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                    Instance = context.Request.Path,
                    Errors = errors
                };

                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/problem+json";

                await context.Response.WriteAsJsonAsync(problemDetails);
            });
        });
    }

    [GeneratedRegex(@"Failed to bind parameter \""(\w+)\[\] \w+\""")]
    private static partial Regex ParameterBindingRegex();
}