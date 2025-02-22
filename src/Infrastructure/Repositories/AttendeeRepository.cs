using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Database;

namespace Infrastructure.Repositories;

public class AttendeeRepository(ConferenceDbContext dbContext) : Repository<Attendee>(dbContext), IAttendeeRepository
{
	
}