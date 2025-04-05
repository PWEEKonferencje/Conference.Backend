using Application.Authentication.OAuthLogin;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class AuthController(IMediator mediator) 
	: ControllerBase
{
	[HttpGet("oauth")]
	public async Task<IActionResult> ExternalLogin([FromQuery] string provider, [FromQuery] string? returnUrl = "")
	{
		var redirectUrl = Url.Action(nameof(OAuthLoginCallback), "Auth", new { returnUrl });
		var result = await mediator.Send(new OAuthLoginCommand(provider, redirectUrl!));
		return Challenge(result.Result, provider);
	}

	[HttpGet("oauth/callback")]
	[ProducesResponseType(typeof(OAuthLoginCallbackResponse), 200)]
	[ProducesResponseType(302)]
	public async Task<IActionResult> OAuthLoginCallback(string? returnUrl = null, string? remoteError = null)
	{
		var response 
			= await mediator.Send(new OAuthLoginCallbackCommand(returnUrl, remoteError));
		if (response.IsSuccess && response.Result.RedirectUrl is not null)
			return Redirect(response.Result.RedirectUrl.ToString());
		return response.ToActionResult();
	}
}