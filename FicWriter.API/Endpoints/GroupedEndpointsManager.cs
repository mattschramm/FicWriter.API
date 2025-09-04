using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace FicWriter.API.Endpoints;

public static class GroupedEndpointsManager
{
    public static IServiceCollection AddGroupedEndpoints(this IServiceCollection services, Assembly assembly)
    {
        var endpointServiceDescriptors = assembly
            .DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                   type.IsAssignableTo(typeof(IEndpoint)))
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
            .ToArray();
        
        services.TryAddEnumerable(endpointServiceDescriptors);
        
        return services;
    }

    public static IApplicationBuilder MapGroupedEndpoints(this WebApplication app)
    {
        var endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();

        var groups = GetGroups(app);

        foreach (var endpoint in endpoints)
        {
            var groupName = endpoint.GetType()
                .GetCustomAttribute<GroupNameAttribute>();

            if (groupName != null)
            {
                if (groups.TryGetValue(groupName.GroupName, out var group))
                {
                    endpoint.MapEndpoint(group);
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(groupName.Tag))
                    {
                        endpoint.MapEndpoint(app.MapGroup(groupName.GroupName)
                            .RequireAuthorization()
                            .WithTags(groupName.Tag));
                    }
                    else
                    {
                        throw new InvalidOperationException(
                            $"Group '{groupName.GroupName}' not found. " +
                            $"Ensure it is defined in {nameof(GetGroups)} method.");
                    }
                }
            }
            else
            {
                throw new InvalidOperationException(
                    $"Endpoint '{endpoint.GetType().Name}' does not have a group defined. " +
                    $"Implement the attribute {nameof(GroupNameAttribute)}.");
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

            [EndpointGroupNames.Works] = app.MapGroup("/works/{workId}")
            .RequireAuthorization()
            .WithTags("Works"),

            [EndpointGroupNames.Users] = app.MapGroup("/user")
            .WithTags("User"),

            [EndpointGroupNames.Dashboard] = app.MapGroup("/dashboard")
            .RequireAuthorization()
            .WithTags("Dashboard"),

            [EndpointGroupNames.WorksGeneral] = app.MapGroup("/works")
            .RequireAuthorization()
            .WithTags("Works")
        };

        return groups;
    }
}
