using System.Net;
using Application.Common.Consts;
using Application.Common.Services;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Domain.Shared;
using MediatR;

namespace Application.Conferences.JoinConference;

public record JoinConferenceCommand(int ConferenceId, Guid AffiliationId)
    : IRequest<ICommandResult<JoinConferenceResponse>>;


internal class JoinConferenceCommandHandler (IAuthenticationService authenticationService, 
    IUnitOfWork unitOfWork, IConferenceRepository conferenceRepository, IAffiliationRepository affiliationRepository,
    IAttendeeRepository attendeeRepository) 
    : IRequestHandler<JoinConferenceCommand, ICommandResult<JoinConferenceResponse>>
{
    public async Task<ICommandResult<JoinConferenceResponse>> Handle(JoinConferenceCommand request, CancellationToken cancellationToken)
    {
        var userAccount = await authenticationService.GetCurrentUser();

        var conference = await conferenceRepository.GetByIdAsync(request.ConferenceId, cancellationToken);
        if (conference is null)
        {
            return CommandResult.Failure<JoinConferenceResponse>(new ErrorResult
            {
                ErrorCode = "ConferenceNotFound",
                StatusCode = HttpStatusCode.NotFound,
            });
        }

        var affiliation = await affiliationRepository.GetByIdAsync(request.AffiliationId, cancellationToken);
        if (affiliation is null)
        {
            return CommandResult.Failure<JoinConferenceResponse>(new ErrorResult
            {
                ErrorCode = "AffiliationNotFound",
                StatusCode = HttpStatusCode.BadRequest,
            });
        }

        var existingAttendee = await attendeeRepository.GetFirstAsync(
            x => x.UserId == userAccount.Id && x.ConferenceId == request.ConferenceId,
            cancellationToken);

        if (existingAttendee is not null)
        {
            return CommandResult.Failure<JoinConferenceResponse>(new ErrorResult
            {
                ErrorCode = "AlreadySigned",
                StatusCode = HttpStatusCode.BadRequest,
            });
        }
        var attendee = Attendee.Create(AttendeeRoleEnum.Participant, userAccount, affiliation);
        attendee.ConferenceId = request.ConferenceId;

        attendeeRepository.Add(attendee, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return CommandResult.Success(new JoinConferenceResponse{});
    }
}



