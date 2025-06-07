using Domain.Enums;

namespace Domain.Entities;

public class PaperStatusRevision
{
	public Guid Id { get; set; }
	public DateTime Timestamp { get; set; } = DateTime.UtcNow;
	public PaperStatus PreviousStatus { get; set; }
	public PaperStatus CurrentStatus { get; set; }
	
	public int PaperId { get; set; }
	public Paper Paper { get; set; } = null!;
}