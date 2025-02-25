using System.Linq.Expressions;
using Domain.Models;

namespace Domain.Repositories;

public interface IRepository<T> where T : class
{
	T Add(T entity, CancellationToken cancellationToken = default);
	T Update(T entity, CancellationToken cancellationToken = default);
	T Delete(T entity, CancellationToken cancellationToken = default);
	Task<T?> GetByIdAsync(object id, CancellationToken cancellationToken = default);
	Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default);
	Task<T?> GetFirstAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

	Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null,
		CancellationToken cancellationToken = default);

	Task<bool> ExistAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

	Task<PagedList<T>> GetPage(int page, int pageSize, Expression<Func<T, bool>>? predicate = null, 
		Expression<Func<T, object>>? orderBy = null, bool orderAsc = true, CancellationToken cancellationToken = default);
}