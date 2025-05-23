using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AttendeeRepository(ConferenceDbContext dbContext) : Repository<Attendee>(dbContext), IAttendeeRepository
{
	public async Task<bool> UserHasRole(int conferenceId, int userId, AttendeeRoleEnum role)
		=> await dbContext.Attendees.AnyAsync(x => 
			x.ConferenceId == conferenceId 
			&& x.UserId == userId 
			&& x.Roles.Any(r => r.RoleEnum == role));
}