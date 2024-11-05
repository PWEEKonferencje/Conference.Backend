using Domain.Entities.Identity;

namespace Domain.Entities;

public class UserProfile
{
	public int Id { get; set; }
	public string Name { get; set; } = default!;
	public string Surname { get; set; } = default!;
	public string? University { get; set; } 
	public string? Degree { get; set; }  
	
	public virtual UserAccount? Account { get; set; }
}