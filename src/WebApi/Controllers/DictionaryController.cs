using System.ComponentModel.DataAnnotations;
using Application.Dictionaries.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DictionaryController(IMediator mediator) : ControllerBase
{
	[HttpGet("university-names")]
	public async Task<IActionResult> GetUniversityNames([FromQuery, Required] string search, [FromQuery] int count = 10)
		=> (await mediator.Send(new GetUniversityNamesQuery(search, count)))
			.ToActionResult();
}