using Domain.Entities.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Authentication;

public interface IAuthenticationService
{
	Task<AuthenticationProperties> ConfigureExternalLoginProperties(string provider, string redirectUrl);
	Task<ExternalLoginInfo?> GetExternalLoginInfoAsync();
	Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent);
	Task<UserAccount?> FindByLoginAsync(string loginProvider, string providerKey);
	Task<UserAccount?> CreateUserFromExternalAsync(ExternalLoginInfo info);
	Task<string> GenerateJwtToken(UserAccount user);
}