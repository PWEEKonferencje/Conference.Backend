using Application.Common.Services;
using AutoMapper;
using Domain.Shared;
using Domain.Repositories;
using MediatR;
using System.Net;

namespace Application.Profiles.GetProfileDetails;

public record GetProfileDetailsQuery(int UserId) : IRequest<IQueryResult<GetProfileDetailsResponse>>;

internal class GetProfileDetailsQueryHandler(IProfileRepository profileRepository, IMapper mapper) 
    : IRequestHandler<GetProfileDetailsQuery, IQueryResult<GetProfileDetailsResponse>>
{
    public async Task<IQueryResult<GetProfileDetailsResponse>> Handle(
        GetProfileDetailsQuery request, 
        CancellationToken cancellationToken)
    {

        var profile = await profileRepository.GetFirstAsync(u => u.Id == request.UserId, cancellationToken);

       if (profile is null)
			return QueryResult.Failure<GetProfileDetailsResponse>(new ErrorResult
            {
                ErrorCode = "Profile not found",
                StatusCode = HttpStatusCode.NotFound,
            });

        var response = mapper.Map<GetProfileDetailsResponse>(profile);

        return QueryResult.Success(response);
    }
}