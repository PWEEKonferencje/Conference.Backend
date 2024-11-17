using System.Security.Claims;
using Domain.Entities;
using Domain.Entities.Identity;
using Infrastructure.Database;
using Microsoft.AspNetCore.Http;

namespace IntegrationTests.Base.Helpers;

/// <summary>
/// Class used to build UserAccount objects for testing purposes. User is set in IHttpContextAccessor.User and in the database.
/// </summary>
public class UserAccountBuilder
{
	private readonly UserAccount _userAccount = new();
	
	private readonly List<Action> _actions = [];
	
	/// <summary>
	/// Sets username of the UserAccount object.
	/// </summary>
	/// <param name="userName">username to set</param>
	public UserAccountBuilder WithUserName(string userName)
	{
		_userAccount.UserName = userName;
		return this;
	}
	
	/// <summary>
	/// Sets Id of the UserAccount object.
	/// </summary>
	/// <param name="id">id to set</param>
	public UserAccountBuilder WithId(string id)
	{
		_userAccount.Id = id;
		return this;
	}
	
	/// <summary>
	/// Sets Email of the UserAccount object.
	/// </summary>
	/// <param name="email">email to set</param>
	public UserAccountBuilder WithEmail(string email)
	{
		_userAccount.Email = email;
		return this;
	}
	
	/// <summary>
	/// Sets OrcidId of the UserAccount object.
	/// </summary>
	/// <param name="orcid">OrcidId to set</param>
	public UserAccountBuilder WithOrcid(string orcid)
	{
		_userAccount.OrcidId = orcid;
		return this;
	}

	public UserAccountBuilder WithProfile(UserProfile profile)
	{
		_userAccount.UserProfile = profile;
		return this;
	}
	
	public UserAccountBuilder AddToDatabase(ConferenceDbContext dbContext)
	{
		_actions.Add(() =>
		{
			dbContext.UserAccounts.Add(_userAccount);
			dbContext.SaveChanges();
		});
		return this;
	}

	public UserAccountBuilder SetInHttpContextAccessor(IHttpContextAccessor httpContextAccessor)
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

	public UserAccount Build()
	{
		_actions.ForEach(x => x.Invoke());
		return _userAccount;
	}
}