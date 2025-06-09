using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Domain.Models;
using Domain.Models.Affiliations;
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
	
	public async Task<PagedList<AttendeeInfoModel>> GetParticipantInfoModels(
	    int conferenceId, int page, int pageSize, CancellationToken cancellationToken)
	{
	    var sourceQuery = dbContext.Attendees
	        .Include(a => a.User)
	        .Include(a => a.UserSnapshot)
	        .Include(a => a.Roles)
	        .Where(a => a.ConferenceId == conferenceId)
	        .OrderBy(a => a.Id);

	    var total = await sourceQuery.CountAsync(cancellationToken);

	    var attendees = await sourceQuery
	        .Skip((page - 1) * pageSize)
	        .Take(pageSize)
	        .ToListAsync(cancellationToken);

	    var result = attendees.Select(a => {
	        return new AttendeeInfoModel
	        {
	            Id = a.Id,
	            Name = a.UserSnapshot?.Name,
	            Degree = a.UserSnapshot?.Degree,
	            Country = a.User?.Country,
	            Position = a.UserSnapshot?.Position,
	            Affiliation = string.IsNullOrWhiteSpace(a.UserSnapshot?.Workplace) &&
	                          string.IsNullOrWhiteSpace(a.UserSnapshot?.Position)
	                ? null
	                : new AffiliationInfoModel
	                {
	                    Workplace = a.UserSnapshot!.Workplace,
	                    Position = a.UserSnapshot!.Position,
	                    IsAcademic = a.UserSnapshot!.IsPositionAcademic
	                },
	            Roles = a.Roles.Select(r => r.RoleEnum.ToString()).ToList(),
	            PapersCount = dbContext.Papers.Count(p => p.CreatorId == a.Id),
	            ReviewsCount = dbContext.Reviews.Count(r => r.ReviewerId == a.Id),
	            RegisteredAt = a.CreatedAt
	        };
	    }).ToList();

	    return new PagedList<AttendeeInfoModel>(result, page, pageSize, total);
	}

}