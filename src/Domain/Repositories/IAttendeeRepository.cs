using Domain.Entities;
using Domain.Enums;

namespace Domain.Repositories;

public interface IAttendeeRepository : IRepository<Attendee>
{
	public Task<bool> UserHasRole(int conferenceId, int userId, AttendeeRoleEnum role);
}
