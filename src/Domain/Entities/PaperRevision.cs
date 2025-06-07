namespace Domain.Entities;

public class PaperFileRevision
{
	public Guid Id { get; set; }
	public DateTime Timestamp { get; set; } = DateTime.UtcNow;

	public Guid FileId { get; set; }
	public File File { get; set; } = null!;
	
	public int PaperId { get; set; }
	public Paper Paper { get; set; } = null!;
}