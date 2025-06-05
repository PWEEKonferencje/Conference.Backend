using System.Net;
using Application.Common.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Domain.Shared;
using MediatR;
using Application.Affiliations.CreateAffiliation;
using Domain.Models.Affiliations;
namespace Application.Affiliations.ModifyAffiliation;

public record ModifyAffiliationCommand(AffiliationModel affiliation) : IRequest<ICommandResult<ModifyAffiliationResponse>>;

internal class ModifyAffiliationCommandHandler(IAuthenticationService authenticationService,IAffiliationRepository affiliationRepository,IMapper mapper,IUnitOfWork unitOfWork) 
	: IRequestHandler<ModifyAffiliationCommand, ICommandResult<ModifyAffiliationResponse>>
{
	public async Task<ICommandResult<ModifyAffiliationResponse>> Handle(ModifyAffiliationCommand request, CancellationToken cancellationToken)
	{
		var user = await authenticationService.GetCurrentUser();
		
        var affiliation = await affiliationRepository.GetByIdAsync(request.affiliation.Id, cancellationToken);

        if (affiliation is null)
        {
            return CommandResult.Failure<ModifyAffiliationResponse>(new ErrorResult
            {
                ErrorCode = "Affiliation Does Not Exist",
                StatusCode = HttpStatusCode.BadRequest,
            });
        }

        if (affiliation.UserId != user.Id)
        {
            return CommandResult.Failure<ModifyAffiliationResponse>(new ErrorResult
            {
                ErrorCode = "Forbidden",
                StatusCode = HttpStatusCode.Forbidden,
            });
        }

		mapper.Map(request.affiliation, affiliation);
        affiliation.UserId = user.Id;
        
		affiliationRepository.Update(affiliation, cancellationToken);
		await unitOfWork.SaveChangesAsync(cancellationToken);
		
		return CommandResult.Success(new ModifyAffiliationResponse());
	}
}