using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Domain.Services.Abstractions;
using Domain.Shared;

namespace Domain.Services.Abstractions;

public interface IRolesService
{
	Task<Result> DeleteRoleAsync(int attendeeId, AttendeeRoleEnum role, CancellationToken cancellationToken = default);
}