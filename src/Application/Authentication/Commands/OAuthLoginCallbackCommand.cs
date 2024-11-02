using System.Net;
using Domain.Models.Authentication;
using Domain.Shared;
using Infrastructure.Authentication;
using MediatR;

namespace Application.Authentication.Commands;

public record OAuthLoginCallbackCommand(string? ReturnUrl, string? RemoteError) : IRequest<ICommandResult<LoginResponse>>;

internal class ExternalLoginCallbackCommandHandler(IAuthenticationService authenticationService)
	: IRequestHandler<OAuthLoginCallbackCommand, ICommandResult<LoginResponse>>
{
	public async Task<ICommandResult<LoginResponse>> Handle(OAuthLoginCallbackCommand request, CancellationToken cancellationToken)
	{
		if (!string.IsNullOrEmpty(request.RemoteError))
		{
			return CommandResult.Failure<LoginResponse>(new ErrorResult
			{
				StatusCode = HttpStatusCode.BadRequest,
				ErrorDescription = "Remote Error",
				ErrorCode = $"There was an external provider error: {request.RemoteError}"
			});
		}

		var info = await authenticationService.GetExternalLoginInfoAsync();
		if (info is null)
		{
			return CommandResult.Failure<LoginResponse>(new ErrorResult
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
			return CommandResult.Success(new LoginResponse
			{
				AccessToken = await authenticationService.GenerateJwtToken(user!),
				IsAccountSetupFinished = user!.UserProfileId is not null
			});
		}
		var newUser = await authenticationService.CreateUserFromExternalAsync(info);
		if(newUser is null)
			return CommandResult.Failure<LoginResponse>(new ErrorResult
			{
				StatusCode = HttpStatusCode.InternalServerError,
				ErrorDescription = "User Creation Error",
				ErrorCode = "There was an error creating the user"
			});
		var token = await authenticationService.GenerateJwtToken(newUser);
		return CommandResult.Success(new LoginResponse
		{
			AccessToken = token,
			IsAccountSetupFinished = false
		});
	}
}