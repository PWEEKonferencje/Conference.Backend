namespace Domain.Entities;

public class AttendeeRole
{
	public Guid Id { get; set; }
	public Enums.AttendeeRoleEnum RoleEnum { get; set; }
	
	public int AttendeeId { get; set; }
	public Attendee Attendee { get; set; } = default!;
}