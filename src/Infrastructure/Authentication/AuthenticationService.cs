using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Common.Configuration;
using Application.Common.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using IAuthenticationService = Application.Common.Services.IAuthenticationService;

namespace Infrastructure.Authentication;

public class AuthenticationService(SignInManager<Identity> signInManager, UserManager<Identity> userManager, 
	AuthenticationConfiguration authenticationConfiguration, IUserContextService userContextService) 
	: IAuthenticationService
{
	public Task<AuthenticationProperties> ConfigureExternalLoginProperties(string provider, string redirectUrl)
	{
		return Task.FromResult(signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl));
	}

	public async Task<ExternalLoginInfo?> GetExternalLoginInfoAsync()
	{
		return await signInManager.GetExternalLoginInfoAsync();
	}

	public async Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent)
	{
		return await signInManager.ExternalLoginSignInAsync(loginProvider, providerKey, isPersistent);
	}

	public async Task<Identity?> FindByLoginAsync(string loginProvider, string providerKey)
	{
		return await userManager.FindByLoginAsync(loginProvider, providerKey);
	}

	public async Task<Identity?> CreateUserFromExternalAsync(ExternalLoginInfo info)
	{
		var user = new Identity
		{
			UserName = info.Principal.FindFirstValue(ClaimTypes.NameIdentifier) ?? $"{info.LoginProvider}_{info.ProviderKey}",
			OAuthId = info.ProviderKey,
			OAuthProvider = info.LoginProvider,
			Email = info.Principal.FindFirstValue(ClaimTypes.Email)
		};
		var result = await userManager.CreateAsync(user);
		if (!result.Succeeded) return null;
		result = await userManager.AddLoginAsync(user, info);
		return result.Succeeded ? user : null;
	}

	public async Task<Identity?> GetCurrentUserAccount()
	{
		var user = userContextService.User;
		if (user is null)
			return null;
		return await userManager.GetUserAsync(userContextService.User!);
	}

	public async Task<string> GenerateJwtToken(Identity user)
	{
		var roles = await userManager.GetRolesAsync(user);
		var claims = new List<Claim>
		{
			new(ClaimTypes.NameIdentifier, user.Id),
			new(ClaimTypes.Name, user.UserName!)
		};
		claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationConfiguration.JwtKey));
		var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
		var expiresDays = authenticationConfiguration.JwtExpireTimeDays;

		var token = new JwtSecurityToken(
			issuer: authenticationConfiguration.JwtIssuer,
			audience: authenticationConfiguration.JwtAudience,
			claims: claims,
			expires: DateTime.UtcNow.AddDays(expiresDays),
			signingCredentials: creds
			);
		return new JwtSecurityTokenHandler().WriteToken(token);
	}
}