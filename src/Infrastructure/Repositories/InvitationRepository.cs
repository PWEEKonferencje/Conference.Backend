using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class InvitationRepository(ConferenceDbContext dbContext) : Repository<Invitation>(dbContext), IInvitationRepository
{
	public Task<Invitation?> GetWithConferenceAndTracksAsync(Guid invitationId, CancellationToken cancellationToken = default) 
		=> dbContext.Invitations
			.Include(i => i.Conference)
			.ThenInclude(x => x.Tracks)
			.FirstOrDefaultAsync(i => i.Id == invitationId, cancellationToken);
}