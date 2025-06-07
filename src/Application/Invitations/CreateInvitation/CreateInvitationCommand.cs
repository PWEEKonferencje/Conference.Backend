using System.Net;
using Application.Common.Services;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Domain.Shared;
using MediatR;

namespace Application.Invitations.CreateInvitation;

public record CreateInvitationCommand(int ConferenceId, InvitationType InvitationType) : IRequest<ICommandResult<CreateInvitationResponse>>;

internal class CreateInvitationCommandHandler(IAuthenticationService authenticationService, IConferenceRepository conferenceRepository,
	IUnitOfWork unitOfWork, IAttendeeRepository attendeeRepository, IInvitationRepository invitationRepository) 
	: IRequestHandler<CreateInvitationCommand, ICommandResult<CreateInvitationResponse>>
{
	public async Task<ICommandResult<CreateInvitationResponse>> Handle(CreateInvitationCommand request, CancellationToken cancellationToken)
	{
		var user = await authenticationService.GetCurrentUser();

		if (request.InvitationType == InvitationType.MultipleUse)
		{
			var existingInvitation = await invitationRepository.GetFirstAsync(x => 
				x.ConferenceId == request.ConferenceId 
				&& x.Type == InvitationType.MultipleUse, 
				cancellationToken);
			if (existingInvitation is not null)
				return CommandResult.Success(new CreateInvitationResponse(existingInvitation.Id.ToString()));
		}
		
		var conference = await conferenceRepository.GetByIdAsync(request.ConferenceId, cancellationToken);
		if (conference is null)
			return CommandResult.Failure<CreateInvitationResponse>(new ErrorResult
			{
				ErrorCode = "ConferenceNotFound",
				ErrorDescription = "Conference not found"
			});
		var attendee = await authenticationService.GetCurrentAttendee(request.ConferenceId);
		var isOrganizer = await attendeeRepository.UserHasRoleAsync(request.ConferenceId, attendee.Id, AttendeeRoleEnum.Organizer);
		if (!isOrganizer)
		{
			return CommandResult.Failure<CreateInvitationResponse>(new ErrorResult
			{
				StatusCode = HttpStatusCode.Forbidden,
				ErrorCode = "InvalidUserRole",
				ErrorDescription = "User does not access to creating invitation in this conference."
			});
		}
		
		var invitation = Invitation.Create(conference, request.InvitationType);
		if (!invitation.IsSuccess)
			return CommandResult.Failure<CreateInvitationResponse>(invitation.ErrorResultOptional!);
		
		invitationRepository.Add(invitation.Value!, cancellationToken);
		await unitOfWork.SaveChangesAsync(cancellationToken);
		return CommandResult.Success(new CreateInvitationResponse(invitation.Value!.Id.ToString()));
	}
}