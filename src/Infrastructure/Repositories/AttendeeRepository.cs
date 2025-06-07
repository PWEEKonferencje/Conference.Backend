using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class AttendeeRepository(ConferenceDbContext dbContext) : Repository<Attendee>(dbContext), IAttendeeRepository
{
	public async Task<bool> UserHasRoleAsync(int userId, int conferenceId, AttendeeRoleEnum role)
	{
		return await dbContext.Attendees
			.Where(a => a.UserId == userId && a.ConferenceId == conferenceId)
			.AnyAsync(a => a.Roles.Any(r => r.RoleEnum == role));
	}

	public async Task<bool> AttendeeHasRoleAsync(int attendeeId, int conferenceId, AttendeeRoleEnum role)
	{
		return await dbContext.Attendees
			.Where(a => a.Id == attendeeId && a.ConferenceId == conferenceId)
			.AnyAsync(a => a.Roles.Any(r => r.RoleEnum == role));
	}
	
	public async Task<Attendee?> GetWithUserSnapshotAsync(Expression<Func<Attendee, bool>> predicate)
	{
		return await dbContext.Attendees
			.Include(x => x.UserSnapshot)
			.FirstOrDefaultAsync(predicate);
	}
	public async Task<Attendee?> GetWithRolesAsync(Expression<Func<Attendee, bool>> predicate)
	{
		return await dbContext.Attendees
			.Include(x => x.Roles)
			.FirstOrDefaultAsync(predicate);
	}
}