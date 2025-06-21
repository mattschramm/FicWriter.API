using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FicWriter.API.Infrastructure.Filters;

public class WorkIdFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (!context.ApiDescription.RelativePath?.Contains("{workId}") ?? true)
            return;

        if (context.ApiDescription.ParameterDescriptions.Any(pd => pd.Name == "workId"))
            return;

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "workId",
            In = ParameterLocation.Path,
            Required = true,
            Schema = new OpenApiSchema
            {
                Type = "string",
                Description = "The encrypted ID of the work."
            }
        });
    }
}
