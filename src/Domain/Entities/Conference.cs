using Domain.ValueObjects;

namespace Domain.Entities;

public class Conference
{
	public int Id { get; set; }
	public string Name { get; set; } = default!;
	
	public string? Description { get; set; }
	public DateTime StartDate { get; set; }
	public DateTime EndDate { get; set; }
	public DateTime RegistrationDeadline { get; set; }
	public DateTime ArticlesDeadline { get; set; }
	public bool IsPublic { get; set; }
	public Address Address { get; set; } = default!;

	public virtual List<Attendee> Attendees { get; set; } = [];
}