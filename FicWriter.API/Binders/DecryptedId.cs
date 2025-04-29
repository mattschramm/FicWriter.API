using Sqids;
using System.Reflection;

namespace FicWriter.API.Binders;

public class DecryptedId
{
    public long Id { get; init; }

    public static ValueTask<DecryptedId?> BindAsync(HttpContext httpContext, ParameterInfo parameter)
    {
        var encoder = httpContext.RequestServices.GetRequiredService<SqidsEncoder<long>>();

        var routeId = httpContext.Request.RouteValues[parameter.Name!]?.ToString();

        if (string.IsNullOrEmpty(routeId))
            return ValueTask.FromResult<DecryptedId?>(null);

        var decryptedId = encoder.Decode(routeId).Single();

        var result = new DecryptedId
        {
            Id = decryptedId
        };

        return ValueTask.FromResult<DecryptedId?>(result);
    }
}
