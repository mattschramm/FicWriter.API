using FicWriter.API.Infrastructure.Mediator.Behaviors;

namespace FicWriter.API.Infrastructure.Mediator;

public static class MediatRDependencyInjection
{
    public static IServiceCollection AddConfiguredMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<Program>();
            cfg.AddOpenBehavior(typeof(ValidateWorkIdBehavior<,>));
        });

        return services;
    }
}
