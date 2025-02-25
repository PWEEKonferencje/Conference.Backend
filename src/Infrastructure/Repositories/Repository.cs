using System.Linq.Expressions;
using Domain.Models;
using Domain.Repositories;
using Infrastructure.Database;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

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
	
	public Task<PagedList<T>> GetPage(int page, int pageSize, Expression<Func<T, bool>>? predicate = null, 
		Expression<Func<T, object>>? orderBy = null, bool orderAsc = true, CancellationToken cancellationToken = default)
	{
		var query = _dbContext.Set<T>().AsQueryable();
		if (predicate is not null)
			query = query.Where(predicate);
		
		if (orderBy is not null && orderAsc)
			query = query.OrderBy(orderBy);
		else if(orderBy is not null && !orderAsc)
			query = query.OrderByDescending(orderBy);
			
		return QueryToPagedList(query, page, pageSize, cancellationToken);
	}

	protected async Task<PagedList<TModel>> QueryToPagedList<TModel>(IQueryable<TModel> query, int page, int pageSize, CancellationToken cancellationToken)
	{
		var totalCount = await query.CountAsync(cancellationToken);
		var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
		
		return new PagedList<TModel>(items, page, pageSize, totalCount);
	}
}