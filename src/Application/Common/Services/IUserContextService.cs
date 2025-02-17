using System.Security.Claims;

namespace Application.Common.Services;

public interface IUserContextService
{
    ClaimsPrincipal? User { get; }
    string? GetUserId();
    string? GetUserName();
    IEnumerable<Claim> GetClaims();
}