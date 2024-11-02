using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Identity;

public class UserAccount : IdentityUser
{
	public string? OAuthProvider { get; set; }
	public string? OAuthId { get; set; }
	
	public int? UserProfileId { get; set; }
	public virtual UserProfile? UserProfile { get; set; }
}