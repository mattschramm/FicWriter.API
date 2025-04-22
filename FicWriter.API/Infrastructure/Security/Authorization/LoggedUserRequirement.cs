using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace FicWriter.API.Infrastructure.Security.Authorization;

public class LoggedUserRequirement : IAuthorizationRequirement
{
}

public class LoggedUserHandler : AuthorizationHandler<LoggedUserRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        LoggedUserRequirement requirement)
    {
        if (context.Resource is HttpContext httpContext)
        {
            var routeId = httpContext.Request.RouteValues["id"]?.ToString();

            var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (Guid.TryParse(routeId, out var routeGuid) &&
                Guid.TryParse(userId, out var userGuid) &&
                routeGuid == userGuid)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail(new AuthorizationFailureReason(this, "You don't have permission"));
            }
        }

        return Task.CompletedTask;
    }
}
