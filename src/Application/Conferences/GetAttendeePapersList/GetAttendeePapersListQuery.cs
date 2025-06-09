using System.Net;
using Application.Common.Services;
using Domain.Repositories;
using Domain.Shared;
using MediatR;

namespace Application.Papers.GetAttendeePapersList;

public record GetAttendeePapersListQuery(int ConferenceId, int Page = 1, int PageSize = 10)
    : IRequest<IQueryResult<GetAttendeePapersListResponse>>;

internal class GetAttendeePapersListQueryHandler(IAuthenticationService authenticationService, IPaperRepository paperRepository,
    IConferenceRepository conferenceRepository)
    : IRequestHandler<GetAttendeePapersListQuery, IQueryResult<GetAttendeePapersListResponse>>
{
    public async Task<IQueryResult<GetAttendeePapersListResponse>> Handle(GetAttendeePapersListQuery request, CancellationToken cancellationToken)
    {
        var conferenceExists = await conferenceRepository.ExistAsync(c => c.Id == request.ConferenceId, cancellationToken);
        if (!conferenceExists)
        {
            return QueryResult.Failure<GetAttendeePapersListResponse>(new ErrorResult
            {
                ErrorCode = "Conference Not Found",
                StatusCode = HttpStatusCode.NotFound
            });
        }
        
        var attendee = await authenticationService.GetCurrentAttendee(request.ConferenceId);

        var papers = await paperRepository.GetAttendeePapers(
            attendee.Id,
            request.Page,
            request.PageSize,
            cancellationToken);

        return QueryResult.Success(new GetAttendeePapersListResponse(papers));
    }
}