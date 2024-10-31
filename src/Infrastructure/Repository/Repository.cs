using System.Linq.Expressions;
using Domain.Repositories;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class Repository<T>(ConferenceDbContext dbContext) : IRepository<T> where T : class
{
	private readonly ConferenceDbContext _dbContext = dbContext;

	public T Add(T entity, CancellationToken cancellationToken = default)
		=> _dbContext.Set<T>().Add(entity).Entity;

	public T Update(T entity, CancellationToken cancellationToken = default)
		=> _dbContext.Set<T>().Update(entity).Entity;

	public T Delete(T entity, CancellationToken cancellationToken = default)
		=> _dbContext.Set<T>().Remove(entity).Entity;

	public async Task<T?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
		=> await _dbContext.Set<T>().FindAsync(id).AsTask();

	public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default)
	{
		return predicate is null
			? await _dbContext.Set<T>().ToListAsync(cancellationToken)
			: await _dbContext.Set<T>().Where(predicate).ToListAsync(cancellationToken);
	}

	public async Task<T?> GetFirstAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
		=> await _dbContext.Set<T>().Where(predicate).FirstOrDefaultAsync(cancellationToken);

	public Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null,
		CancellationToken cancellationToken = default)
	{
		return predicate is null
			? _dbContext.Set<T>().CountAsync(cancellationToken)
			: _dbContext.Set<T>().CountAsync(predicate, cancellationToken);
	}

	public Task<bool> ExistAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
		=> _dbContext.Set<T>().AnyAsync(predicate, cancellationToken);
}