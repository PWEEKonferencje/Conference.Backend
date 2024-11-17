using Domain.Repositories;
using Domain.Shared;
using Infrastructure.Authentication;
using MediatR;

namespace Application.Profiles.Commands;

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
		var result = account.SetEmail(command.Email);
		await unitOfWork.SaveChangesAsync(cancellationToken);	
		return result.IsSuccess ? 
			CommandResult.Success<object?>(null) : 
			CommandResult.Failure<object?>(result.ErrorResultOptional!);
	}
}