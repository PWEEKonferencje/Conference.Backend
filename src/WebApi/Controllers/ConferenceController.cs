using Application.Attendees.JoinConference;
using Application.Conferences.CreateConference;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConferenceController(IMediator mediator) : ControllerBase
{
	[HttpPost("/create")]
	[ProducesResponseType<CreateConferenceResponse>(201)]
	public async Task<IActionResult> CreateConference([FromBody] CreateConferenceCommand command)
	{
		return (await mediator.Send(command)).ToActionResult(201);
	}
	
	[HttpPost("{conferenceId}/sign")]
	[ProducesResponseType<JoinConferenceResponse>(200)]
	public async Task<IActionResult> CreateConferenceAttendee([FromRoute] int conferenceId, [FromQuery] Guid affiliationId)
	{
		return (await mediator.Send(new JoinConferenceCommand(conferenceId, affiliationId))).ToActionResult();
	}	
}