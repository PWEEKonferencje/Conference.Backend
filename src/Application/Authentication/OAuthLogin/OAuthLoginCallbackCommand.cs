using System.Net;
using Application.Common.Services;
using Domain.Shared;
using MediatR;

namespace Application.Authentication.OAuthLogin;

public record OAuthLoginCallbackCommand(string? ReturnUrl, string? RemoteError) : IRequest<ICommandResult<OAuthLoginCallbackResponse>>;

internal class ExternalLoginCallbackCommandHandler(IAuthenticationService authenticationService)
	: IRequestHandler<OAuthLoginCallbackCommand, ICommandResult<OAuthLoginCallbackResponse>>
{
	public async Task<ICommandResult<OAuthLoginCallbackResponse>> Handle(OAuthLoginCallbackCommand request, CancellationToken cancellationToken)
	{
		if (!string.IsNullOrEmpty(request.RemoteError))
		{
			return CommandResult.Failure<OAuthLoginCallbackResponse>(new ErrorResult
			{
				StatusCode = HttpStatusCode.BadRequest,
				ErrorDescription = "Remote Error",
				ErrorCode = $"There was an external provider error: {request.RemoteError}"
			});
		}

		var user = await authenticationService.ExternalLoginOrRegisterAsync();
		if (user is null)
			return CommandResult.Failure<OAuthLoginCallbackResponse>(new ErrorResult
			{
				StatusCode = HttpStatusCode.BadRequest,
				ErrorCode = "LoginError",
				ErrorDescription = "There was an error logging in with the external provider.",
			});
		
		var token = await authenticationService.GenerateJwtToken(user);
		var isEmailProvided = !string.IsNullOrEmpty(user.Email);
		var isAccountSetupFinished = user.IsAccountSetupFinished();
		
		var response = new OAuthLoginCallbackResponse(
			token, 
			isAccountSetupFinished, 
			isEmailProvided, 
			request.ReturnUrl);
		
		return CommandResult.Success(response);
	}
}