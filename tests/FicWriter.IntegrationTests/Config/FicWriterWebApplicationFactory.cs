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
    private List<API.Models.Work> _works = [];

    public string UserPassword => _password;
    public string UserName => _user.Name;
    public string UserEmail => _user.Email;
    public string RefreshToken => _refreshToken.Token;
    public Guid UserIdentifier => _user.UserIdentifier;
    public string WorkTitle => _work.Title;
    public string WorkDescription => _work.Description;
    public string EncryptedWorkId => SqidsEncoderBuilder.Build().Encode(_work.Id);
    public long WorkId => _work.Id;
    public List<API.Models.Work> Works => _works;

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

        _works = [];
        _works.Add(_work);
        _works.Add(WorkBuilder.Build(_user, id: 2));
        _works.Add(WorkBuilder.Build(_user, id: 3));

        dbContext.Users.Add(_user);

        dbContext.RefreshTokens.Add(_refreshToken);

        dbContext.Works.AddRange(_works);

        dbContext.SaveChanges();
    }
}
