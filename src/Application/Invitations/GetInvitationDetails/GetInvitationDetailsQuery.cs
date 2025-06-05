using System.Net;
using Application.Conferences.Models;
using Domain.Repositories;
using Domain.Shared;
using MediatR;

namespace Application.Invitations.GetInvitationDetails;

public record GetInvitationDetailsQuery(Guid InvitationId) 
	: IRequest<IQueryResult<GetInvitationDetailsResponse>>;

internal class GetInvitationDetailsQueryHandler(IInvitationRepository invitationRepository)
	: IRequestHandler<GetInvitationDetailsQuery, IQueryResult<GetInvitationDetailsResponse>>
{
	public async Task<IQueryResult<GetInvitationDetailsResponse>> Handle(GetInvitationDetailsQuery request, CancellationToken cancellationToken)
	{
		var invitation = await invitationRepository.GetWithConferenceAndTracksAsync(request.InvitationId, cancellationToken);
		if (invitation is null)
		{
			return QueryResult.Failure<GetInvitationDetailsResponse>(new ErrorResult
			{
				StatusCode = HttpStatusCode.NotFound,
				ErrorCode = "Invitation Not Found",
				ErrorDescription = "Invitation was not found."
			});
		}
		var conference = invitation.Conference;

		var response = new GetInvitationDetailsResponse(
			new ConferenceDetailsDto(
				conference.Name, 
				conference.Description,
				conference.ArticlesDeadline, 
				conference.RegistrationDeadline),
			invitation.Conference.RegistrationDeadline < DateTime.UtcNow, 
			conference.Tracks.Select(x => new TrackDto(x.Name, x.Description)).ToList());
		
		return QueryResult.Success(response);
	}
}