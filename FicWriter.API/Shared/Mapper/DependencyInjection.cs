namespace FicWriter.API.Shared.Mapper;

public static class DependencyInjection
{
    public static IServiceCollection AddMappers(this IServiceCollection services)
    {
        var mapperServiceDescriptors = typeof(IFeatureMapper).Assembly
            .DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                   type.IsAssignableTo(typeof(IFeatureMapper)))
            .ToArray();

        foreach (var mapper in mapperServiceDescriptors)
        {
            services.AddScoped(mapper);
        }

        return services;
    }
}
