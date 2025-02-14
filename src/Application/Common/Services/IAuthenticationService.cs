using Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace Application.Common.Services;

public interface IAuthenticationService
{
	Task<AuthenticationProperties> ConfigureExternalLoginProperties(string provider, string redirectUrl);
	Task<ExternalLoginInfo?> GetExternalLoginInfoAsync();
	Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent);
	Task<Identity?> FindByLoginAsync(string loginProvider, string providerKey);
	Task<Identity?> CreateUserFromExternalAsync(ExternalLoginInfo info);
	Task<Identity?> GetCurrentUserAccount();
	Task<string> GenerateJwtToken(Identity user);
}