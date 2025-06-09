using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Domain.Models;
using Domain.Models.Conference;

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
	
	public async Task<bool> IsUserAttendeeOfConference(int userId, int conferenceId, CancellationToken cancellationToken)
	{
		return await dbContext.Attendees
			.AnyAsync(a => a.UserId == userId && a.ConferenceId == conferenceId, cancellationToken);
	}
	
	public async Task<PagedList<AttendeeInfoModel>> GetParticipantInfoModels(
		int conferenceId, int page, int pageSize, CancellationToken cancellationToken)
	{
		var baseQuery = dbContext.Attendees
			.Where(a => a.ConferenceId == conferenceId)
			.OrderBy(a => a.Id);

		var total = await baseQuery.CountAsync(cancellationToken);

		var attendeeIds = await baseQuery
			.Skip((page - 1) * pageSize)
			.Take(pageSize)
			.Select(a => a.Id)
			.ToListAsync(cancellationToken);

		var attendees = await dbContext.Attendees
			.Where(a => attendeeIds.Contains(a.Id))
			.Include(a => a.User)
			.Include(a => a.UserSnapshot)
			.Include(a => a.Roles)
			.ToListAsync(cancellationToken);

		var result = attendees.Select(a => new AttendeeInfoModel
		{
			Id = a.Id,
			Name = a.UserSnapshot?.Name,
			Degree = a.UserSnapshot?.Degree,
			Country = a.User?.Country,
			Position = a.UserSnapshot?.Position,
			Workplace = a.UserSnapshot?.Workplace,
			IsAcademic = a.UserSnapshot?.IsPositionAcademic,
			Roles = a.Roles.Select(r => r.RoleEnum.ToString()).ToList(),
			PapersCount = dbContext.Papers.Count(p => p.CreatorId == a.Id),
			ReviewsCount = dbContext.Reviews.Count(r => r.ReviewerId == a.Id),
			RegisteredAt = a.CreatedAt
		}).ToList();

		return new PagedList<AttendeeInfoModel>(result, page, pageSize, total);
	}
}