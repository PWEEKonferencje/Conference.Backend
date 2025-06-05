using Application.Common.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using Domain.Shared;
using Domain.Models.Affiliations;
using MediatR;
using Application.Affiliations.GetAffiliations;
namespace Application.Affiliations.GetAffiliations;

public record GetAffiliationsQuery() : IRequest<IQueryResult<GetAffiliationsResponse>>;

internal class GetAffiliationsQueryHandler(IAuthenticationService authenticationService,IAffiliationRepository affiliationRepository,IMapper mapper)
	: IRequestHandler<GetAffiliationsQuery, IQueryResult<GetAffiliationsResponse>>
{
	public async Task<IQueryResult<GetAffiliationsResponse>> Handle(GetAffiliationsQuery request,CancellationToken cancellationToken)
	{
		var user = await authenticationService.GetCurrentUser();

		var affiliations = await affiliationRepository.GetAllAsync(x => x.UserId == user.Id, cancellationToken);

		var response = mapper.Map<GetAffiliationsResponse>(affiliations);

		return QueryResult.Success(response);	
	}
}