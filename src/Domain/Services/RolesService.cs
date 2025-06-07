using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Domain.Services.Abstractions;
using Domain.Shared;

namespace Domain.Services;

public class RolesService(
    IAttendeeRepository attendeeRepository,
    IUnitOfWork unitOfWork,
    IConferenceRepository conferenceRepository)
    : IRolesService
{
    public async Task<Result> DeleteRoleAsync(int attendeeId, AttendeeRoleEnum role, CancellationToken cancellationToken = default)
    {
        var attendee = await attendeeRepository.GetWithRolesAsync(a => a.Id == attendeeId);
        if (attendee == null)
            return Result.Failure(new List<Error> { new Error("Attendee not found.") });

        if (!attendee.Roles.Any(r => r.RoleEnum == role))
            return Result.Failure(new List<Error> { new Error("Attendee does not have this role.") });

        if (attendee.Roles.Count == 1)
            return Result.Failure(new List<Error> { new Error("Cannot delete the only role.") });

        if (role == AttendeeRoleEnum.Participant && attendee.Roles.Any(r => r.RoleEnum == AttendeeRoleEnum.CommitteeMember))
            return Result.Failure(new List<Error> { new Error("Cannot delete participant role while committee member role exists.") });

        attendee.Roles.RemoveAll(r => r.RoleEnum == role);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}



