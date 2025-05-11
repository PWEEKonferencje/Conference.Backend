namespace Domain.Entities;

public class Affiliation
{
	public Guid Id { get; set; }
	public string Workplace { get; set; } = default!;
	public string Position { get; set; } = default!;
	public bool IsAcademic { get; set; }
	public int UserId { get; set; }
	public User User { get; set; } 
}