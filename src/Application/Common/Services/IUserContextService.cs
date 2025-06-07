using System.Security.Claims;

namespace Application.Common.Services;

public interface IUserContextService
{
    ClaimsPrincipal? User { get; }
    string? GetIdentityId();
    string? GetUserName();
    IEnumerable<Claim> GetClaims();
    int? GetAttendeeId();
}