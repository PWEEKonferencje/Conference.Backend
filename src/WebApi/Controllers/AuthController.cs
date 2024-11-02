using Application.Authentication.Commands;
using Domain.Entities.Identity;
using Domain.Models.Authentication;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
[ProducesResponseType(typeof(ErrorResult), 400)]
[ProducesResponseType(typeof(ErrorResult), 500)]
public class AuthController(UserManager<UserAccount> userManager, SignInManager<UserAccount> signInManager, IMediator mediator) 
	: ControllerBase
{
	[HttpGet("oauth")]
	public async Task<IActionResult> ExternalLogin([FromQuery] string provider, [FromQuery] string? returnUrl = "/")
	{
		var redirectUrl = Url.Action(nameof(OAuthLoginCallback), "Auth", new { returnUrl });
		var result = await mediator.Send(new OAuthLoginCommand(provider, redirectUrl!));
		return Challenge(result.Result, provider);
	}

	[HttpGet("oauth/callback")]
	[ProducesResponseType(typeof(LoginResponse), 200)]
	public async Task<IActionResult> OAuthLoginCallback(string? returnUrl = null, string? remoteError = null)
	{
		return (await mediator.Send(new OAuthLoginCallbackCommand(returnUrl, remoteError))).ToActionResult();

	}
}