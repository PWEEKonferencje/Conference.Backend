using Application.Profiles.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions;
using Microsoft.AspNetCore.Authorization;
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
}