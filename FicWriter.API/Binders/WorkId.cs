using FicWriter.API.Infrastructure.Security.IdEncoder;
using Sqids;
using System.Reflection;

namespace FicWriter.API.Binders;

public class WorkId
{
    public long Value { get; set; }

    private const long InvalidWorkIdValue = -1;

    public static ValueTask<WorkId?> BindAsync(HttpContext context, ParameterInfo parameter)
    {

        var encoder = context.RequestServices.GetRequiredService<SqidsEncoder<long>>();
        var workIdValue = context.Request.RouteValues["workId"]?.ToString();

        WorkId result = new();

        if (string.IsNullOrEmpty(workIdValue))
        {
            result.Value = InvalidWorkIdValue;
            return ValueTask.FromResult<WorkId?>(result);
        }

        try
        {
            result.Value = encoder.DecodeSingle(workIdValue);
        }
        catch (Exception)
        {
            result.Value = InvalidWorkIdValue;
        }

        return ValueTask.FromResult<WorkId?>(result);
    }
}
