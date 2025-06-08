using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Database;

namespace Infrastructure.Repositories;

public class ReviewRepository(ConferenceDbContext dbContext) : Repository<Review>(dbContext), IReviewRepository
{
	
}