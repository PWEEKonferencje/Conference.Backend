using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Database;

namespace Infrastructure.Repository;

public class ProfileRepository(ConferenceDbContext dbContext) : Repository<UserProfile>(dbContext), IProfileRepository
{
    
}