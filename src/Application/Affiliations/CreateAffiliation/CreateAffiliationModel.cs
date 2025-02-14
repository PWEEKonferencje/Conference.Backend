namespace Application.Affiliations.CreateAffiliation;

public class CreateAffiliationModel
{
	public string Workplace { get; set; } = default!;
	public string Position { get; set; } = default!;
	public string Description { get; set; } = default!;
	public bool IsAcademic { get; set; }
}