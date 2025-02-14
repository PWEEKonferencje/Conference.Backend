using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class Identity : IdentityUser
{
	public string? OAuthProvider { get; set; }
	public string? OAuthId { get; set; }

	public int? UserProfileId { get; set; }
	public virtual User? UserProfile { get; set; }
}