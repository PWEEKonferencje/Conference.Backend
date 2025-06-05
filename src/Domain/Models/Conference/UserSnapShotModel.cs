namespace Domain.Models.Conference;

public class UserSnapshotModel
{
	public string? Name { get; set; }
	public string? Surname { get; set; }
	public string? OrcidId { get; set; }
	public string? Degree { get; set; }
	public string? Workplace { get; set; }
	public string? Position { get; set; }
	public bool? IsPositionAcademic { get; set; }
    public DateTime CreatedAt { get; set; }
}