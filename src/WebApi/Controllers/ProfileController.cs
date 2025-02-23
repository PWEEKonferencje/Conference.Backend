using Application.Profiles.CreateProfile;
using Application.Profiles.SetProfileEmail;
using Application.Profiles.SetProfileOrcid;
using Application.Affiliations.GetAffiliations;
using Application.Affiliations.CreateAffiliation;
using Application.Affiliations.ModifyAffiliation;
using Application.Affiliations.DeleteAffiliation;
using Domain.Models.Affiliations;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProfileController (IMediator mediator): ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(CreateProfileResponse), 201)]
    public async Task<IActionResult> CreateProfile([FromBody] CreateProfileCommand profileRequest)
    {
        return (await mediator.Send(profileRequest)).ToActionResult(201);
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

    [HttpGet("affiliations")]
    [ProducesResponseType(typeof(GetAffiliationsResponse), 200)]
    public async Task<IActionResult> GetAffiliationsList()
    {
        return (await mediator.Send(new GetAffiliationsQuery())).ToActionResult();
    }   

    [HttpPost("affiliations")]
    public async Task<IActionResult> CreateAffiliation([FromBody] CreateAffiliationModel createAffiliationModel)
    {
        var affiliationCommand = new CreateAffiliationCommand(createAffiliationModel);
        return (await mediator.Send(affiliationCommand)).ToActionResult(201);
    }   

    [HttpPut("affiliations")]
    public async Task<IActionResult> ModifyAffiliation([FromBody] AffiliationModel affiliationModel)
    {
        var affiliationCommand = new ModifyAffiliationCommand(affiliationModel);
        return (await mediator.Send(affiliationCommand)).ToActionResult();
    } 

    [HttpDelete("affiliations/{id}")]
    public async Task<IActionResult> DeleteAffiliation(Guid id)
    {
        return (await mediator.Send(new DeleteAffiliationCommand(id))).ToActionResult(204);
    }
}