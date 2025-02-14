using System.Security.Claims;
using Domain.Entities;
using Infrastructure.Database;
using Microsoft.AspNetCore.Http;

namespace IntegrationTests.Base.Helpers;

/// <summary>
/// Class used to build UserAccount objects for testing purposes. User is set in IHttpContextAccessor.User and in the database.
/// </summary>
public class IdentityBuilder
{
	private readonly Identity _userAccount = new();
	
	private readonly List<Action> _actions = [];
	
	/// <summary>
	/// Sets username of the UserAccount object.
	/// </summary>
	/// <param name="userName">username to set</param>
	public IdentityBuilder WithUserName(string userName)
	{
		_userAccount.UserName = userName;
		return this;
	}
	
	/// <summary>
	/// Sets Id of the UserAccount object.
	/// </summary>
	/// <param name="id">id to set</param>
	public IdentityBuilder WithId(string id)
	{
		_userAccount.Id = id;
		return this;
	}
	
	/// <summary>
	/// Sets Email of the UserAccount object.
	/// </summary>
	/// <param name="email">email to set</param>
	public IdentityBuilder WithEmail(string email)
	{
		_userAccount.Email = email;
		return this;
	}

	public IdentityBuilder WithProfile(User profile)
	{
		_userAccount.UserProfile = profile;
		return this;
	}
	
	public IdentityBuilder AddToDatabase(ConferenceDbContext dbContext)
	{
		_actions.Add(() =>
		{
			dbContext.UserIdentities.Add(_userAccount);
			dbContext.SaveChanges();
		});
		return this;
	}

	public IdentityBuilder SetInHttpContextAccessor(IHttpContextAccessor httpContextAccessor)
	{
		_actions.Add(() =>
		{
			httpContextAccessor.HttpContext ??= new DefaultHttpContext();
			httpContextAccessor.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new[]
			{
				new Claim(ClaimTypes.NameIdentifier, _userAccount.Id),
				new Claim(ClaimTypes.Name, _userAccount.UserName!)
			}));
		});
		return this;
	}

	public Identity Build()
	{
		_actions.ForEach(x => x.Invoke());
		return _userAccount;
	}
}