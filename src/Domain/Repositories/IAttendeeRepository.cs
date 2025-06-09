using Domain.Entities;
using Domain.Enums;
using System.Linq.Expressions;
using Domain.Models;

namespace Domain.Repositories;

public interface IAttendeeRepository : IRepository<Attendee>
{
	Task<bool> UserHasRoleAsync(int userId, int conferenceId, AttendeeRoleEnum role);
	Task<Attendee?> GetWithUserSnapshotAsync(Expression<Func<Attendee, bool>> predicate);
	Task<bool> AttendeeHasRoleAsync(int attendeeId, AttendeeRoleEnum role);

	Task<PagedList<Attendee>> GetParticipantsWithDetails(int conferenceId, int page, int pageSize,
		CancellationToken cancellationToken);
}
