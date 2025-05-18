using Domain.Entities;
using Domain.Enums;

namespace Domain.Repositories;

public interface IAttendeeRepository : IRepository<Attendee>
{
    Task<bool> UserHasRoleAsync(int userId, int conferenceId, AttendeeRoleEnum role);
}
