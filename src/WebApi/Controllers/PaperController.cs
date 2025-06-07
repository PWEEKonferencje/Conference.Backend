using Application.Common.Consts;
using Application.Papers.CreatePaper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaperController(IMediator mediator)
{
	[HttpPost]
	[ProducesResponseType<CreatePaperResponse>(StatusCodes.Status201Created)]
	public async Task<IActionResult> CreatePaper([FromHeader(Name = Headers.AttendeeId)] string attendeeId, 
		[FromBody] CreatePaperCommand command)
	{
		return (await mediator.Send(command)).ToActionResult(201);
	}
}