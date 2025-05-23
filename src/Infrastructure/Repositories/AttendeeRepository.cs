using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AttendeeRepository(ConferenceDbContext dbContext) : Repository<Attendee>(dbContext), IAttendeeRepository
{
	public async Task<bool> UserHasRoleAsync(int userId, int conferenceId, AttendeeRoleEnum role)
	{
		return await dbContext.Attendees
			.Where(a => a.UserId == userId && a.ConferenceId == conferenceId)
			.AnyAsync(a => a.Roles.Any(r => r.RoleEnum == role));
	}
}