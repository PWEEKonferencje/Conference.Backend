using Domain.Enums;

namespace Domain.Entities;

public class Attendee
{
	public int Id { get; set; }

	public virtual List<AttendeeRole> Roles { get; set; } = default!;

	public int ConferenceId { get; set; }
	public Conference Conference { get; set; } = default!;

	public int UserId { get; set; }
	public User User { get; set; } = default!;

	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

	public Guid UserSnapshotId { get; set; }
	public UserSnapshot UserSnapshot { get; set; } = default!;

	public static Attendee Create(AttendeeRoleEnum role, User user, Affiliation affiliation)
	{
		return new Attendee
		{
			UserSnapshot = UserSnapshot.Create(user, affiliation),
			UserId = user.Id,
			Roles = [ new AttendeeRole{RoleEnum = role } ]
		};
	}
}