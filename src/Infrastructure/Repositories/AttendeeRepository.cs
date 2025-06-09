using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Domain.Models;

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

	public Task<bool> AttendeeHasRoleAsync(int attendeeId, AttendeeRoleEnum role)
	{
		return dbContext.Attendees.AnyAsync(x => 
			x.Id == attendeeId 
		    && x.Roles.Any(r => r.RoleEnum == role));
	}
	
	public async Task<PagedList<Attendee>> GetParticipantsWithDetails(
		int conferenceId, int page, int pageSize, CancellationToken cancellationToken)
	{
		var query = dbContext.Attendees
			.Include(a => a.User)
			.Include(a => a.UserSnapshot)
			.Where(a => a.ConferenceId == conferenceId &&
			            a.Roles.Any(r => r.RoleEnum == AttendeeRoleEnum.Participant))
			.OrderBy(a => a.Id);

		var total = await query.CountAsync(cancellationToken);
		var items = await query
			.Skip((page - 1) * pageSize)
			.Take(pageSize)
			.ToListAsync(cancellationToken);

		return new PagedList<Attendee>(items, total, page, pageSize);
	}
}