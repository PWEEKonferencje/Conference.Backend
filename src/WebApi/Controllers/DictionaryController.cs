using System.ComponentModel.DataAnnotations;
using Application.Dictionaries.Queries;
using Domain.Shared;
using Infrastructure.Dictionaries.UniversityNames;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(typeof(ErrorResult), 400)]
[ProducesResponseType(typeof(ErrorResult), 500)]
public class DictionaryController(IMediator mediator) : ControllerBase
{
	[HttpGet("university-names")]
	[ProducesResponseType(typeof(IReadOnlyList<UniversityNameModel>), 200)]
	public async Task<IActionResult> GetUniversityNames([FromQuery, Required] string search, [FromQuery] int count = 10)
		=> (await mediator.Send(new GetUniversityNamesQuery(search, count)))
			.ToActionResult();
}