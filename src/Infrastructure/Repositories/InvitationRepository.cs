using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Database;

namespace Infrastructure.Repositories;

public class InvitationRepository(ConferenceDbContext dbContext) : Repository<Invitation>(dbContext), IInvitationRepository
{
	
}