using CommonTestUtils.Models;
using CommonTestUtils.Services;
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
    private RefreshToken _refreshToken = default!;
    private API.Models.Work _work = default!;

    public string GetUserPassword() => _password;
    public string GetUserName() => _user.Name;
    public string GetUserEmail() => _user.Email;
    public string GetRefreshToken() => _refreshToken.Token;
    public Guid GetUserIdentifier() => _user.UserIdentifier;
    public string GetWorkTitle() => _work.Title;
    public string GetWorkDescription() => _work.Description;
    public string GetEncryptedWorkId() => SqidsEncoderBuilder.Build().Encode(_work.Id);
    public long GetWorkId() => _work.Id;

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

        _refreshToken = RefreshTokenBuilder.Build();
        _refreshToken.UserId = _user.Id;
        _refreshToken.User = _user;

        _work = WorkBuilder.Build(_user);

        dbContext.Users.Add(_user);

        dbContext.RefreshTokens.Add(_refreshToken);

        dbContext.Works.Add(_work);

        dbContext.SaveChanges();
    }
}
