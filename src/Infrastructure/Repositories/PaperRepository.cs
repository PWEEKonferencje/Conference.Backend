using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Database;

namespace Infrastructure.Repositories;

public class PaperRepository(ConferenceDbContext dbContext) : Repository<Paper>(dbContext), IPaperRepository
{
	
}