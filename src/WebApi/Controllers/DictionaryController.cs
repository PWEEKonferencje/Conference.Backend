using Application.Common.Services;
using Application.Dictionaries.Queries;
using Domain.Models.Dictionary;
using Infrastructure.Dictionaries.UniversityNames;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class DictionaryController(IMediator mediator) : ControllerBase
{
	[HttpGet("university-names")]
	[ProducesResponseType(typeof(IReadOnlyList<UniversityNameModel>), 200)]
	public async Task<IActionResult> GetUniversityNames([FromQuery] string search, [FromQuery] int count = 10)
	{
		return (await mediator.Send(new GetUniversityNamesQuery(search, count)))
			.ToActionResult();
	} 
}