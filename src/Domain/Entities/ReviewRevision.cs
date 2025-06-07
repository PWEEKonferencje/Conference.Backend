namespace Domain.Entities;

public class ReviewRevision
{
	public int Id { get; set; }
	public DateTime Timestamp { get; set; } = DateTime.UtcNow;
	public string PreviousContent { get; set; } = string.Empty;
	public string CurrentContent { get; set; } = string.Empty;

	public int ReviewId { get; set; }
	public Review Review { get; set; } = null!;
}