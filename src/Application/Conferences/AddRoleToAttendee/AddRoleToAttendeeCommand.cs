using Application.Common.Services;
using Application.Conferences.CreateConference;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Domain.Shared;
using MediatR;
using System.Net;



namespace Application.Conferences.AddRoleToAttendee;

public record AddRoleToAttendeeCommand(int ConferenceId, int AttendeeId, AttendeeRoleEnum Role) : IRequest<ICommandResult<AddRoleToAttendeeResponse>>;

internal class AddRoleToAttendeeCommandHandler(IAuthenticationService authenticationService, IAttendeeRepository attendeeRepository, IConferenceRepository conferenceRepository, IUnitOfWork unitOfWork) 
    : IRequestHandler<AddRoleToAttendeeCommand, ICommandResult<AddRoleToAttendeeResponse>>
{
    public async Task<ICommandResult<AddRoleToAttendeeResponse>> Handle(AddRoleToAttendeeCommand request, CancellationToken cancellationToken)
    {

	    var currentAttendee = await authenticationService.GetCurrentAttendee(request.ConferenceId);

		var isOrganizer = await attendeeRepository.UserHasRoleAsync(request.ConferenceId, currentAttendee.Id, AttendeeRoleEnum.Organizer);
		if (!isOrganizer)
		{
			return CommandResult.Failure<AddRoleToAttendeeResponse>(new ErrorResult
			{
				StatusCode = HttpStatusCode.Forbidden,
				ErrorCode = "InvalidUserRole",
				ErrorDescription = "User does not access to creating invitation in this conference."
			});
		}
        
        var attendee = await attendeeRepository.GetFirstAsync(a => a.Id == request.AttendeeId && a.ConferenceId == request.ConferenceId, cancellationToken);
        if (attendee is null)
            return CommandResult.Failure<AddRoleToAttendeeResponse>(
                ErrorResult.DomainError([new Error("Attendee not found.", nameof(request.AttendeeId))]));

        if (attendee.Roles.Any(r => r.RoleEnum == request.Role))
            return CommandResult.Failure<AddRoleToAttendeeResponse>(new ErrorResult
            {
                ErrorCode = "AttendeeAlreadyHasRole"
            });
       
        attendee.Roles.Add(new AttendeeRole { RoleEnum = request.Role });
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return CommandResult.Success(new AddRoleToAttendeeResponse());
    }
}