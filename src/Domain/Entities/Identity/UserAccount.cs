using Domain.Shared;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Identity;

public class UserAccount : IdentityUser
{
	public string? OAuthProvider { get; set; }
	public string? OAuthId { get; set; }
	public string? OrcidId { get; set; }
	
	public int? UserProfileId { get; set; }
	public virtual UserProfile? UserProfile { get; set; }

	public Result SetOrcid(string orcidId)
	{
		if (OrcidId is not null)
			return Result.Failure([new Error("Orcid is already set", nameof(OrcidId))]);
		OrcidId = orcidId;
		return Result.Success();
	}

	public Result SetEmail(string email)
	{
		if (Email is not null)
			return Result.Failure([new Error("Email is already set", nameof(Email))]);
		Email = email;
		return Result.Success();
	}

	public bool CreateProfile(string name, string surname, string? university, string? degree)
	{
		if (UserProfileId is not null)
			return false;
		var profile = new UserProfile
		{
			Name = name,
			Surname = surname,
			University = university,
			Degree = degree
		};
		UserProfile = profile;
		return true;
	}
}