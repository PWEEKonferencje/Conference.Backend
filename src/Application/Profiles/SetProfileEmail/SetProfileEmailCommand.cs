using Application.Common.Services;
using Domain.Repositories;
using Domain.Shared;
using MediatR;

namespace Application.Profiles.SetProfileEmail;

public record SetProfileEmailCommand(string Email) : IRequest<ICommandResult<object?>>;

internal class SetProfileEmailCommandHandler(IAuthenticationService authenticationService, 
	IUnitOfWork unitOfWork) 
	: IRequestHandler<SetProfileEmailCommand, ICommandResult<object?>>
{
	public async Task<ICommandResult<object?>> Handle(SetProfileEmailCommand command, CancellationToken cancellationToken)
	{
		var account = await authenticationService.GetCurrentUserAccount();
		if(account is null)
			return CommandResult.Failure<object?>(ErrorResult.AuthorizationError);
		account.Email = command.Email;
		await unitOfWork.SaveChangesAsync(cancellationToken);
		return CommandResult.Success<object?>(null);
	}
}