using Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace Application.Common.Services;

public interface IAuthenticationService
{
	Task<AuthenticationProperties> ConfigureExternalLoginProperties(string provider, string redirectUrl);
	Task<Identity> GetCurrentIdentity();
	Task<User> GetCurrentUser();
	Task<string> GenerateJwtToken(Identity user);
	Task<Identity?> ExternalLoginOrRegisterAsync();
	Task<Attendee> GetCurrentAttendee(int conferenceId);
}