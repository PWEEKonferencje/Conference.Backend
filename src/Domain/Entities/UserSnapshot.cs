namespace Domain.Entities;

public class UserSnapshot
{
	public Guid Id { get; set; }
	public string? Name { get; set; }
	public string? Surname { get; set; }
	public string? OrcidId { get; set; }
	public string? Degree { get; set; }
	public string? Workplace { get; set; }
	public string? Position { get; set; }
	public bool? IsPositionAcademic { get; set; }
	
	public int UserId { get; set; }
	public User User { get; set; } = default!;
	
	public static UserSnapshot Create(User user, Affiliation affiliation)
	{
		return new UserSnapshot
		{
			Name = user.Name,
			Surname = user.Surname,
			OrcidId = user.OrcidId,
			Degree = user.Degree,
			Workplace = affiliation.Workplace,
			Position = affiliation.Position,
			IsPositionAcademic = affiliation.IsAcademic,
			UserId = user.Id
		};
	}
}