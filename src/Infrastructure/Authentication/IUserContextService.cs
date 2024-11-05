using System.Security.Claims;

namespace Infrastructure.Authentication;

public interface IUserContextService
{
    ClaimsPrincipal? User { get; }
    int? GetUserId();
    string? GetUserName();
    IEnumerable<Claim> GetClaims();
}