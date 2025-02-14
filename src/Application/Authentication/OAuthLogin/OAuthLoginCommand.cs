using Domain.Shared;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using IAuthenticationService = Application.Common.Services.IAuthenticationService;

namespace Application.Authentication.OAuthLogin;

public record OAuthLoginCommand(string Provider, string ReturnUrl) : IRequest<ICommandResult<AuthenticationProperties>>;

internal class OAuthLoginCommandHandler(IValidator<OAuthLoginCommand> validator, IAuthenticationService authenticationService)
	: IRequestHandler<OAuthLoginCommand, ICommandResult<AuthenticationProperties>>
{
	public async Task<ICommandResult<AuthenticationProperties>> Handle(OAuthLoginCommand request, CancellationToken cancellationToken)
	{
		var properties = await authenticationService.ConfigureExternalLoginProperties(request.Provider, request.ReturnUrl);
		return CommandResult.Success(properties);
	}
}