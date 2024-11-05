using Application.Profiles.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Domain.Models.Profile;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProfileController (IMediator mediator): ControllerBase
{
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> CreateProfile([FromBody] CreateProfileRequest profileRequest)
    {
        return (await mediator.Send(new CreateProfileCommand(profileRequest))).ToActionResult(201);
    }
}