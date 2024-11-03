using Domain.Entities.Identity;

namespace Domain.Entities;

public class UserProfile
{
	public int Id { get; set; }
	public virtual UserAccount? Account { get; set; }
}