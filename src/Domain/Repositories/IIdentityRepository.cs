using Domain.Entities;

namespace Domain.Repositories;

public interface IIdentityRepository : IRepository<Identity>
{
	Task<Identity?> GetWithUserAsync(string id);
}