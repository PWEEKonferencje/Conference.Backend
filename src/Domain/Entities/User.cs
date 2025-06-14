namespace Domain.Entities;

public class User
{
	public int Id { get; set; }
	public string? OrcidId { get; set; }
	public string? Name { get; set; }
	public string? Surname { get; set; }
	public string? Degree { get; set; }

	public string? Email { get; set; }
	public string? Phone { get; set; }
	public string? ResearchInterest { get; set; }
	public string? Country { get; set; }

	public bool IsProfileSetUp { get; set; }

	public virtual List<Identity> Account { get; set; } = [];
	public virtual List<Affiliation> Affiliations { get; set; } = [];
}