using Application.Common.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Domain.Shared;
using MediatR;

namespace Application.Affiliations.CreateAffiliation;

public record CreateAffiliationCommand(CreateAffiliationModel Affiliation) : IRequest<ICommandResult<CreateAffiliationResponse>>;

internal class CreateAffiliationCommandHandler(IAuthenticationService authenticationService,IAffiliationRepository affiliationRepository,IMapper mapper,IUnitOfWork unitOfWork) 
	: IRequestHandler<CreateAffiliationCommand, ICommandResult<CreateAffiliationResponse>>
{
	public async Task<ICommandResult<CreateAffiliationResponse>> Handle(CreateAffiliationCommand request, CancellationToken cancellationToken)
	{
		var user = await authenticationService.GetCurrentUser();
		
		var affiliation = mapper.Map<Affiliation>(request.Affiliation);

		affiliation.UserId=user.Id;

		affiliationRepository.Add(affiliation, cancellationToken);
		await unitOfWork.SaveChangesAsync(cancellationToken);
		
		return CommandResult.Success(new CreateAffiliationResponse());
	}
}