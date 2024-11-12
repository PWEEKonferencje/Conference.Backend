using Domain.Entities.Identity;
using Domain.Repositories;
using Domain.Shared;
using Infrastructure.Authentication;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Application.Profiles.Commands;

public record SetProfileOrcidCommand(string OrcidId) : IRequest<ICommandResult<object?>>;

internal class SetProfileOrcidCommandHandler(IAuthenticationService authenticationService,
	IUnitOfWork unitOfWork)
	: IRequestHandler<SetProfileOrcidCommand, ICommandResult<object?>>
{
	public async Task<ICommandResult<object?>> Handle(SetProfileOrcidCommand request, CancellationToken cancellationToken)
	{
		var account = await authenticationService.GetCurrentUserAccount();
		if (account is null)
			return CommandResult.Failure<object?>(ErrorResult.AuthorizationError);
		var result = account.SetOrcid(request.OrcidId);
		if(!result.IsSuccess)
			return CommandResult.Failure<object?>(result.ErrorResultOptional ?? ErrorResult.GenericError);
		await unitOfWork.SaveChangesAsync(cancellationToken);
		return CommandResult.Success<object?>(null);
	}
}