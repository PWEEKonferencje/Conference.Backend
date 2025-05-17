using Application.Common.Services;
using Application.Conferences.CreateConference;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Domain.Shared;
using MediatR;

namespace Application.Conferences.AddConferenceTrack;

public record AddConferenceTrackCommand : IRequest<ICommandResult<AddConferenceTrackResponse>>
{
    public int ConferenceId { get; init; }
    public string Name { get; init; } = default!;
    public string Description { get; set; } = default!;
}

internal class AddConferenceTrackCommandHandler(IAuthenticationService authenticationService, IMapper mapper, 
    IConferenceRepository conferenceRepository, IUnitOfWork unitOfWork) 
    : IRequestHandler<AddConferenceTrackCommand, ICommandResult<AddConferenceTrackResponse>>
{
    public async Task<ICommandResult<AddConferenceTrackResponse>> Handle(AddConferenceTrackCommand request, CancellationToken cancellationToken)
    {
        var user = await authenticationService.GetCurrentUser();
        if (user is null || user.IsProfileSetUp is false)
            return CommandResult.Failure<AddConferenceTrackResponse>(ErrorResult.AuthorizationError);
        
        var conference = await conferenceRepository.GetByIdAsync(request.ConferenceId, cancellationToken);
        if (conference is null)
            return CommandResult.Failure<AddConferenceTrackResponse>(
                ErrorResult.DomainError([new Error("Conference not found.", nameof(request.ConferenceId))]));
        var isOrganizer = conference.Attendees.Any(attendee =>
            attendee.UserId == user.Id &&
            attendee.Roles.Any(role => role.RoleEnum == AttendeeRoleEnum.Organizer));

        if (!isOrganizer)
            return CommandResult.Failure<AddConferenceTrackResponse>(
                ErrorResult.AuthorizationError);
        
        var track = mapper.Map<ConferenceTrack>(request);
        conference.ConferenceTracks.Add(track);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return CommandResult.Success(new AddConferenceTrackResponse { Id = track.Id });
    }
}