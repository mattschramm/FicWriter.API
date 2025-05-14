using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace FicWriter.API.Endpoints;

public static class DependencyInjectionGrouped
{
    public static IServiceCollection AddGroupedEndpoints(this IServiceCollection services, Assembly assembly)
    {
        var endpointServiceDescriptors = assembly
            .DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                   type.IsAssignableTo(typeof(IGroupedEndpoint)))
            .Select(type => ServiceDescriptor.Transient(typeof(IGroupedEndpoint), type))
            .ToArray();
        
        services.TryAddEnumerable(endpointServiceDescriptors);
        
        return services;
    }

    public static IApplicationBuilder MapGroupedEndpoints(this WebApplication app)
    {
        var endpoints = app.Services.GetRequiredService<IEnumerable<IGroupedEndpoint>>();

        var groups = GetGroups(app);

        foreach (var endpoint in endpoints)
        {
            var groupName = endpoint.GetType()
                .GetCustomAttribute<EndpointGroupAttribute>();

            if (groupName != null)
            {
                if (groups.TryGetValue(groupName.GroupName, out var group))
                {
                    endpoint.MapEndpoint(group);
                }
                else
                {
                    throw new InvalidOperationException($"Group '{groupName.GroupName}' not found for endpoint '{endpoint.GetType().Name}'.");
                }
            }
            else
            {
                throw new InvalidOperationException(
                    $"Endpoint '{endpoint.GetType().Name}' does not have a group defined. " +
                    $"Implement the attribute {nameof(EndpointGroupAttribute)}.");
            }
        }

        return app;
    }

    private static Dictionary<string, RouteGroupBuilder> GetGroups(WebApplication app)
    {
        var groups = new Dictionary<string, RouteGroupBuilder>
        {
            [EndpointGroupNames.Drafts] = app.MapGroup("/works/{workId}/drafts")
            .RequireAuthorization()
            .WithTags("Drafts"),

            [EndpointGroupNames.Works] = app.MapGroup("/works")
            .RequireAuthorization()
            .WithTags("Works")
        };

        return groups;
    }
}
