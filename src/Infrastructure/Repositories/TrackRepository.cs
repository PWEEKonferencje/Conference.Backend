using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Database;

namespace Infrastructure.Repositories;

public class TrackRepository(ConferenceDbContext dbContext) : Repository<Track>(dbContext), ITrackRepository
{
	
}