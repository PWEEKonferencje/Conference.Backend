using System.Net;
using Domain.Repositories;
using Domain.Shared;
using MediatR;

namespace Application.Conferences.GetAttendeesInfoList;

public record GetAttendeesInfoListQuery(int ConferenceId, int Page = 1, int PageSize = 20) : IRequest<IQueryResult<GetAttendeesInfoListResponse>>;

internal class GetAttendeesQueryHandler(IAttendeeRepository attendeeRepository, IConferenceRepository conferenceRepository)
    : IRequestHandler<GetAttendeesInfoListQuery, IQueryResult<GetAttendeesInfoListResponse>>
{
    public async Task<IQueryResult<GetAttendeesInfoListResponse>> Handle(GetAttendeesInfoListQuery request, CancellationToken cancellationToken)
    {
        var conferenceExists = await conferenceRepository
            .ExistAsync(c => c.Id == request.ConferenceId, cancellationToken);

        if (!conferenceExists)
        {
            return QueryResult.Failure<GetAttendeesInfoListResponse>(new ErrorResult
            {
                ErrorCode = "ConferenceNotFound",
                StatusCode = HttpStatusCode.NotFound,
            });
        }

        var pagedResult = await attendeeRepository.GetParticipantInfoModels(
            request.ConferenceId,
            request.Page,
            request.PageSize,
            cancellationToken);
        
        var response = new GetAttendeesInfoListResponse(pagedResult);

        return QueryResult.Success(response);
    }
}
