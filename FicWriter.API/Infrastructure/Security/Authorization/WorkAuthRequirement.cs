using FicWriter.API.Infrastructure.Data;
using FicWriter.API.Infrastructure.Security.IdEncoder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Sqids;
using System.Security.Claims;

namespace FicWriter.API.Infrastructure.Security.Authorization;

public class WorkAuthRequirement : IAuthorizationRequirement { }

public class WorkAuthHandler(
    FicWriterDbContext dbContext,
    SqidsEncoder<long> encoder,
    IHttpContextAccessor httpContextAccessor) : AuthorizationHandler<WorkAuthRequirement>
{
    private readonly FicWriterDbContext _dbContext = dbContext;
    private readonly SqidsEncoder<long> _encoder = encoder;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        WorkAuthRequirement requirement)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        
        if (httpContext is null)
        {
            context.Fail(new AuthorizationFailureReason(this, "HTTP context is not available."));
            return;
        }

        var userIdClaim = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (!Guid.TryParse(userIdClaim, out Guid userIdentifier))
        {
            context.Fail(new AuthorizationFailureReason(this, "Invalid user identifier claim."));
            return;
        }

        var workIdRouteValue = httpContext.Request.RouteValues["workId"]?.ToString();
        if (string.IsNullOrEmpty(workIdRouteValue))
        {
            context.Fail(new AuthorizationFailureReason(this, "Work ID is required in the route."));
            return;
        }

        var decryptedWorkId = _encoder.DecodeSingle(workIdRouteValue);

        var isOwner = await _dbContext.Works
            .AnyAsync(work =>
                work.Id == decryptedWorkId &&
                work.User.UserIdentifier == userIdentifier);

        if (isOwner)
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail(new AuthorizationFailureReason(this, "You do not have permission to access this work."));
        }
    }
}