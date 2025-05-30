using System.Linq.Expressions;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class IdentityRepository(ConferenceDbContext dbContext) : Repository<Identity>(dbContext), IIdentityRepository
{
	public async Task<Identity?> GetWithUserAsync(Expression<Func<Identity, bool>> predicate)
	{
		return await dbContext.UserIdentities
			.Include(x => x.UserProfile)
			.FirstOrDefaultAsync(predicate);
	}
}