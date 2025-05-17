using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Database;

namespace Infrastructure.Repositories;

public class ConferenceTrackRepository(ConferenceDbContext dbContext) : Repository<ConferenceTrack>(dbContext), IConferenceTrackRepository
{
	
}