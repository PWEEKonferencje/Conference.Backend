using Domain.Entities;

namespace Domain.Repositories;

public interface IInvitationRepository : IRepository<Invitation>
{
	Task<Invitation?> GetWithConferenceAndTracksAsync(Guid invitationId, CancellationToken cancellationToken = default);
}