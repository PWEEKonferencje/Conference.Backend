using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Database;

namespace Infrastructure.Repositories;

public class ConferenceRepository(ConferenceDbContext dbContext) : Repository<Conference>(dbContext), IConferenceRepository
{
	
}