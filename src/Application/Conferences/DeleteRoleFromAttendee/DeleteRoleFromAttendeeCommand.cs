using Application.Common.Services;
using Application.Conferences.CreateConference;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Domain.Shared;
using MediatR;
using System.Net;
using Domain.Services.Abstractions;


namespace Application.Conferences.DeleteRoleFromAttendee;

public record DeleteRoleFromAttendeeCommand(int ConferenceId, int AttendeeId, AttendeeRoleEnum Role) : IRequest<ICommandResult<DeleteRoleFromAttendeeResponse>>;

internal class DeleteRoleFromAttendeeCommandHandler(IRolesService rolesService, IAuthenticationService authenticationService, IAttendeeRepository attendeeRepository) 
    : IRequestHandler<DeleteRoleFromAttendeeCommand, ICommandResult<DeleteRoleFromAttendeeResponse>>
{
    public async Task<ICommandResult<DeleteRoleFromAttendeeResponse>> Handle(DeleteRoleFromAttendeeCommand request, CancellationToken cancellationToken)
    {
		var attendee = await authenticationService.GetCurrentAttendee(request.ConferenceId);
		var isOrganizer = await attendeeRepository.UserHasRoleAsync(request.ConferenceId, attendee.Id, AttendeeRoleEnum.Organizer);
		if (!isOrganizer)
		{
			return CommandResult.Failure<DeleteRoleFromAttendeeResponse>(new ErrorResult
			{
				StatusCode = HttpStatusCode.Forbidden,
				ErrorCode = "InvalidUserRole",
				ErrorDescription = "Attendee does not have permission to delete roles in this conference."
			});
		}

        var result = await rolesService.DeleteRoleAsync(request.AttendeeId, request.Role, cancellationToken);
        if (!result.IsSuccess)
			return CommandResult.Failure<DeleteRoleFromAttendeeResponse>(result.ErrorResultOptional!);

        return CommandResult.Success(new DeleteRoleFromAttendeeResponse());
    }
}