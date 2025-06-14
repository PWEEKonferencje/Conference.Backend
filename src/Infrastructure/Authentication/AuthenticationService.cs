using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using Application.Common.Configuration;
using Application.Common.Exceptions;
using Application.Common.Services;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using IAuthenticationService = Application.Common.Services.IAuthenticationService;

namespace Infrastructure.Authentication;

public class AuthenticationService(SignInManager<Identity> signInManager, UserManager<Identity> userManager, 
	AuthenticationConfiguration authenticationConfiguration, IUserContextService userContextService, 
	IIdentityRepository identityRepository, IAttendeeRepository attendeeRepository) 
	: IAuthenticationService
{
	public Task<AuthenticationProperties> ConfigureExternalLoginProperties(string provider, string redirectUrl)
	{
		return Task.FromResult(signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl));
	}

	public async Task<Identity?> ExternalLoginOrRegisterAsync()
	{
		var loginInfo = await signInManager.GetExternalLoginInfoAsync();
		if (loginInfo is null)
			return null;
		var signInResult = await signInManager.ExternalLoginSignInAsync(loginInfo.LoginProvider, loginInfo.ProviderKey, false);
		if (signInResult.Succeeded)
			return await Login(loginInfo);
		return await Register(loginInfo);
	}

	private async Task<Identity?> Login(ExternalLoginInfo info)
	{
		return await identityRepository.GetWithUserAsync(x 
			=> x.OAuthId == info.ProviderKey && x.OAuthProvider == info.LoginProvider);
	}

	private async Task<Identity?> Register(ExternalLoginInfo info)
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

	public async Task<Identity> GetCurrentIdentity()
	{
		var user = userContextService.User;
		if (user is null)
			throw new AuthenticationException("User not found");
		return await userManager.GetUserAsync(userContextService.User!) ?? throw new AuthenticationException("User not found");
	}

	public async Task<User> GetCurrentUser()
	{
		var identityId = userContextService.GetIdentityId();
		if (identityId is null)
			throw new AuthenticationException();
		var identity = await identityRepository.GetWithUserAsync(x => x.Id == identityId);
		if (identity?.UserProfile is null || !identity.UserProfile.IsProfileSetUp)
			throw new ProfileNotSetUpException();
		return identity.UserProfile;
	}

	public async Task<Attendee> GetCurrentAttendee(int conferenceId)
	{
		var attendeeIdHeader = userContextService.GetAttendeeId();
		if (attendeeIdHeader is null)
			throw new AuthenticationException();
		var user = await GetCurrentUser();
		var attendee = await attendeeRepository.GetFirstAsync(x => 
			x.ConferenceId == conferenceId 
			&& x.UserId == user.Id 
			&& x.Id == attendeeIdHeader);
		if (attendee is null)
			throw new AuthenticationException();
		return attendee;
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