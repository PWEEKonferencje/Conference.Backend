using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Database;

namespace Infrastructure.Repositories;

public class AffiliationRepository(ConferenceDbContext dbContext) : Repository<Affiliation>(dbContext), IAffiliationRepository
{
	
}