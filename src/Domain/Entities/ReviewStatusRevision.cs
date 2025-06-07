using Domain.Enums;

namespace Domain.Entities;

public class ReviewStatusRevision
{
	public Guid Id { get; set; }
	public ReviewStatus PreviousStatus { get; set; }
	public ReviewStatus CurrentStatus { get; set; }
	public DateTime Timestamp { get; set; } = DateTime.UtcNow;

	public int ReviewId { get; set; }
	public Review Review { get; set; } = null!;
}