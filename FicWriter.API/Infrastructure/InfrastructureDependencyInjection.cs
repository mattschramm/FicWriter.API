using FicWriter.API.Binders;
using FicWriter.API.Infrastructure.Data;
using FicWriter.API.Infrastructure.Data.Repositories.Tokens;
using FicWriter.API.Infrastructure.Data.Repositories.Users;
using FicWriter.API.Infrastructure.Data.Repositories.Works;
using FicWriter.API.Infrastructure.Security.Password;
using FicWriter.API.Infrastructure.Security.Tokens.Access;
using FicWriter.API.Infrastructure.Security.Tokens.Refresh;
using FicWriter.API.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Sqids;

namespace FicWriter.API.Infrastructure;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        if (!configuration.GetValue<bool>("InMemoryTest"))
            AddDbContext(services, configuration);

        AddRepositories(services);
        AddPasswordHasher(services);
        AddTokenGenerator(services, configuration);
        AddSqids(services, configuration);

        return services;
    }

    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<FicWriterDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging()
                .UseSnakeCaseNamingConvention());
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IUserReadOnly, UserRepository>();
        services.AddScoped<IUserWriteOnly, UserRepository>();
        services.AddScoped<IUserUpdateOnly, UserRepository>();

        services.AddScoped<ITokenWriteOnly, TokenRepository>();
        services.AddScoped<ITokenReadOnly, TokenRepository>();
        services.AddScoped<ITokenUpdateOnly, TokenRepository>();

        services.AddScoped<ICurrentUser, CurrentUser>();

        services.AddScoped<IWorkWriteOnly, WorkRepository>();
        services.AddScoped<IWorkReadOnly, WorkRepository>();
    }

    private static void AddPasswordHasher(IServiceCollection services)
    {
        services.AddScoped<IPasswordHasher, PasswordHasher>();
    }

    private static void AddTokenGenerator(IServiceCollection services, IConfiguration configuration)
    {
        var key = configuration["Jwt:Key"]!;
        var expirationTime = configuration.GetValue<uint>("Jwt:ExpirationTime");
        var issuer = configuration["Jwt:Issuer"]!;
        var audience = configuration["Jwt:Audience"]!;

        services.AddScoped<IAccessTokenGenerator>(options => new JwtTokenGenerator(key, expirationTime, issuer, audience));

        services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();
    }

    private static void AddSqids(IServiceCollection services, IConfiguration configuration)
    {
        var alphabet = configuration["Sqids:Alphabet"]!;
        var length = configuration.GetValue<int>("Sqids:MinLength");

        services.AddSingleton(new SqidsEncoder<long>(new()
        {
            Alphabet = alphabet,
            MinLength = length,
        }));
    }
}
