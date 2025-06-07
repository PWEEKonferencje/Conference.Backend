using Domain.Enums;

namespace Domain.Entities;

public class Review
{
	public int Id { get; set; }
	public ReviewStatus Status { get; set; } = ReviewStatus.Requested;
	public string? Comment { get; set; } = string.Empty;

	public int ReviewerId { get; set; }
	public Attendee Reviewer { get; set; } = null!;

	public int PaperId { get; set; }
	public Paper Paper { get; set; } = null!;

	public List<ReviewStatusRevision> StatusRevisions { get; set; } = [];
	public List<ReviewRevision> Revisions { get; set; } = [];
}