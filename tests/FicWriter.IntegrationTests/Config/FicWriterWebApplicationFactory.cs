using CommonTestUtils.Models;
using FicWriter.API.Infrastructure.Data;
using FicWriter.API.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FicWriter.IntegrationTests.Config;

public class FicWriterWebApplicationFactory : WebApplicationFactory<Program>
{
    private API.Models.User _user = default!;
    private string _password = string.Empty;

    public string GetUserPassword() => _password;
    public string GetUserName() => _user.Name;
    public string GetUserEmail() => _user.Email;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test")
            .ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(service => service.ServiceType == typeof(FicWriterDbContext));
                
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                services.AddDbContext<FicWriterDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting")
                        .UseInternalServiceProvider(provider);
                });

                using var scope = services.BuildServiceProvider().CreateScope();

                var dbContext = scope.ServiceProvider.GetRequiredService<FicWriterDbContext>();

                dbContext.Database.EnsureDeleted();

                StartDatabase(dbContext);
            });
    }

    private void StartDatabase(FicWriterDbContext dbContext)
    {
        (_user, _password) = UserBuilder.Build();

        dbContext.Users.Add(_user);

        dbContext.SaveChanges();
    }
}
