using FicWriter.API.Infrastructure.Data.Repositories.Users;

namespace FicWriter.API.Infrastructure.Data;

public static class DataExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserReadOnly, UserRepository>();
        services.AddScoped<IUserWriteOnly, UserRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
