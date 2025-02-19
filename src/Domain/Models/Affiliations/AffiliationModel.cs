namespace Domain.Models.Affiliations;

public class AffiliationModel
{
	public Guid Id { get; set; }
	public string Workplace { get; set; } = default!;
	public string Position { get; set; } = default!;
	public string? Description { get; set; }
	public bool IsAcademic { get; set; }
}