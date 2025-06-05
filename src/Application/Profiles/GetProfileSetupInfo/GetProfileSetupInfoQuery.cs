using Application.Common.Services;
using Domain.Repositories;
using Domain.Shared;
using MediatR;

namespace Application.Profiles.GetProfileSetupInfo;

public record GetProfileSetupInfoQuery() : IRequest<IQueryResult<GetProfileSetupInfoResponse>>;

internal class GetProfileSetupInfoQueryHandler(
    IAuthenticationService authenticationService, IProfileRepository profileRepository
) : IRequestHandler<GetProfileSetupInfoQuery, IQueryResult<GetProfileSetupInfoResponse>>
{
    public async Task<IQueryResult<GetProfileSetupInfoResponse>> Handle(
        GetProfileSetupInfoQuery request, 
        CancellationToken cancellationToken)
    {
        var account = await authenticationService.GetCurrentIdentity();
        
        var user = await authenticationService.GetCurrentUser();

        var response = new GetProfileSetupInfoResponse
        {
            IsAccountSetupFinished = account.IsAccountSetupFinished(),
            IsEmailProvided = !string.IsNullOrEmpty(account.Email),
            IsOrcidProvided = !string.IsNullOrEmpty(user.OrcidId)
        };

        return QueryResult.Success(response);
    }
}