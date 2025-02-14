using System.Security.Claims;
using Application.Common.Services;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Authentication;

public class UserContextService (IHttpContextAccessor httpContextAccessor) : IUserContextService
{
    public ClaimsPrincipal? User => httpContextAccessor.HttpContext?.User;

    public int? GetUserId()
    {
        if (User is null)
            return null;

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        return userIdClaim != null ? int.Parse(userIdClaim.Value) : (int?)null;
    }

    public string? GetUserName()
    {
        return User?.Identity?.Name;
    }

    public IEnumerable<Claim> GetClaims()
    {
        return User?.Claims ?? Enumerable.Empty<Claim>();
    }
}