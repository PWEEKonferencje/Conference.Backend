using Application.Profiles.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions;
using Domain.Models.Profile;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProfileController (IMediator mediator): ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(CreateProfileResponse), 201)]
    public async Task<IActionResult> CreateProfile([FromBody] CreateProfileRequest profileRequest)
    {
        return (await mediator.Send(new CreateProfileCommand(profileRequest))).ToActionResult(201);
    }
    
    [HttpPost("email")]
    public async Task<IActionResult> AddEmail([FromBody] SetProfileEmailCommand emailCommand)
    {
        return (await mediator.Send(emailCommand)).ToActionResult();
    }
    
    [HttpPost("orcid")]
    public async Task<IActionResult> AddOrcid([FromBody] SetProfileOrcidCommand orcidCommand)
    {
        return (await mediator.Send(orcidCommand)).ToActionResult();
    }
}