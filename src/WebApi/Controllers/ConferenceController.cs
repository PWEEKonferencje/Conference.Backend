using Application.Conferences.CreateConference;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConferenceController(IMediator mediator) : ControllerBase
{
	[HttpPost("create")]
	[ProducesResponseType<CreateConferenceResponse>(201)]
	public async Task<IActionResult> CreateConference([FromBody] CreateConferenceCommand command)
	{
		return (await mediator.Send(command)).ToActionResult(201);
	}
}