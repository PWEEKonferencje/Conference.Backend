using Application.Common.Services;
using Domain.Shared;
using MediatR;

namespace Application.Profiles.GetProfileSetupInfo;

public record GetProfileSetupInfoQuery() : IRequest<IQueryResult<GetProfileSetupInfoResponse>>;

internal class GetProfileSetupInfoQueryHandler(
    IAuthenticationService authenticationService
) : IRequestHandler<GetProfileSetupInfoQuery, IQueryResult<GetProfileSetupInfoResponse>>
{
    public async Task<IQueryResult<GetProfileSetupInfoResponse>> Handle(
        GetProfileSetupInfoQuery request, 
        CancellationToken cancellationToken)
    {
        var account = await authenticationService.GetCurrentIdentity();
        
        var user = await authenticationService.GetCurrentUser();
        
        if (user is null || account is null )
            return QueryResult.Failure<GetProfileSetupInfoResponse>(ErrorResult.AuthorizationError);

        var response = new GetProfileSetupInfoResponse
        {
            IsAccountSetupFinished = account.IsAccountSetupFinished(),
            IsEmailProvided = !string.IsNullOrEmpty(account.Email),
            IsOrcidProvided = !string.IsNullOrEmpty(user.OrcidId)
        };

        return QueryResult.Success(response);
    }
}