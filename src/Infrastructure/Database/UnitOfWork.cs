using Domain.Repositories;

namespace Infrastructure.Database;

public class UnitOfWork(ConferenceDbContext dbContext) : IUnitOfWork
{
	private readonly ConferenceDbContext _dbContext = dbContext;

	public Task SaveChangesAsync(CancellationToken cancellationToken = default)
		=> _dbContext.SaveChangesAsync(cancellationToken);
}