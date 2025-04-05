using System.Linq.Expressions;
using Domain.Entities;

namespace Domain.Repositories;

public interface IIdentityRepository : IRepository<Identity>
{
	Task<Identity?> GetWithUserAsync(Expression<Func<Identity, bool>> predicate);
}