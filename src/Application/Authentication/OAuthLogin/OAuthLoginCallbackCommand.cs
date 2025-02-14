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
		var info = await authenticationService.GetExternalLoginInfoAsync();
		if (info is null)
		{
			return CommandResult.Failure<OAuthLoginCallbackResponse>(new ErrorResult
			{
				StatusCode = HttpStatusCode.BadRequest,
				ErrorDescription = "External Login Info Error",
				ErrorCode = "There was an error getting the external login info"
			});
		}
		
		var signInResult = await authenticationService.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
		if (signInResult.Succeeded)
		{
			var user = await authenticationService.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
			return CommandResult.Success(new OAuthLoginCallbackResponse
			{
				AccessToken = await authenticationService.GenerateJwtToken(user!),
				IsAccountSetupFinished = user!.UserProfileId is not null
			});
		}
		var newUser = await authenticationService.CreateUserFromExternalAsync(info);
		if(newUser is null)
			return CommandResult.Failure<OAuthLoginCallbackResponse>(new ErrorResult
			{
				StatusCode = HttpStatusCode.InternalServerError,
				ErrorDescription = "User Creation Error",
				ErrorCode = "There was an error creating the user"
			});
		var token = await authenticationService.GenerateJwtToken(newUser);
		var isEmailProvided = !string.IsNullOrEmpty(newUser.Email);
		
		return CommandResult.Success(new OAuthLoginCallbackResponse
		{
			AccessToken = token,
			IsAccountSetupFinished = false,
			IsEmailProvided = isEmailProvided
		});
	}
}