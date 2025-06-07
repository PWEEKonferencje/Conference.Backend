using Domain.Entities;
using Domain.Enums;
using System.Linq.Expressions;

namespace Domain.Repositories;

public interface IAttendeeRepository : IRepository<Attendee>
{
	Task<bool> UserHasRoleAsync(int userId, int conferenceId, AttendeeRoleEnum role);
	Task<Attendee?> GetWithUserSnapshotAsync(Expression<Func<Attendee, bool>> predicate);
	Task<Attendee?> GetWithRolesAsync(Expression<Func<Attendee, bool>> predicate);
}
