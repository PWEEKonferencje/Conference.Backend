using System.ComponentModel.DataAnnotations;
using Application.Common.Consts;
using Application.Conferences.AddConferenceTrack;
using Application.Conferences.JoinConference;
using Application.Conferences.CreateConference;
using Application.Invitations.CreateInvitation;
using Application.Invitations.GetInvitationDetails;
using Application.Conferences.GetAttendeeSnapshot;
using Domain.Enums;
using Domain.Shared;
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
	
	[HttpPost("{conferenceId}/join")]
	[ProducesResponseType<JoinConferenceResponse>(200)]
	public async Task<IActionResult> JoinConference([FromRoute] int conferenceId, [FromQuery] Guid affiliationId)
	{
		return (await mediator.Send(new JoinConferenceCommand(conferenceId, affiliationId))).ToActionResult();
	}
	
	[HttpPost("track/add")]
	[ProducesResponseType<AddConferenceTrackResponse>(200)]
	public async Task<IActionResult> AddConferenceTrack([FromHeader(Name = Headers.AttendeeId), Required] string attendeeId, [FromBody] AddConferenceTrackCommand command)
	{
		return (await mediator.Send(command)).ToActionResult();
	}
	
	[HttpPost("{conferenceId}/createInvitation")]
	[ProducesResponseType<CreateInvitationResponse>(200)]
	[ProducesResponseType<ErrorResult>(403)]
	public async Task<IActionResult> CreateInvitation([FromRoute] int conferenceId, [FromQuery] InvitationType invitationType)
	{
		return (await mediator.Send(new CreateInvitationCommand(conferenceId, invitationType))).ToActionResult();
	}

	[HttpGet("invitation/{invitationId:guid}")]
	[ProducesResponseType<GetInvitationDetailsResponse>(200)]
	public async Task<IActionResult> GetInvitation([FromRoute] Guid invitationId)
	{
		return (await mediator.Send(new GetInvitationDetailsQuery(invitationId))).ToActionResult();
	}

	[HttpGet("attendee/{attendeeId}/snapshot")]
	[ProducesResponseType<GetAttendeeSnapshotResponse>(200)]
	public async Task<IActionResult> GetAttendeeSnapshot([FromRoute] int attendeeId, [FromQuery] int conferenceId)
	{
		return (await mediator.Send(new GetAttendeeSnapshotQuery(conferenceId, attendeeId))).ToActionResult();
	}
}