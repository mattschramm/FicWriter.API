using FicWriter.API.Infrastructure.Services;
using FicWriter.API.Models;
using Moq;

namespace CommonTestUtils.Services;

public static class CurrentUserBuilder
{
    public static ICurrentUser Build(User user)
    {
        var currentUser = new Mock<ICurrentUser>();
        
        currentUser.Setup(x => x.GetCurrentUser()).ReturnsAsync(user);
        
        return currentUser.Object;
    }
}
