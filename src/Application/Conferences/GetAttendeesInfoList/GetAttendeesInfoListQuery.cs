using System.Net;
using Application.Common.Services;
using Domain.Repositories;
using Domain.Shared;
using MediatR;

namespace Application.Conferences.GetAttendeesInfoList;

public record GetAttendeesInfoListQuery(int ConferenceId, int Page = 1, int PageSize = 20) : IRequest<IQueryResult<GetAttendeesInfoListResponse>>;

internal class GetAttendeesInfoListQueryHandler(IAttendeeRepository attendeeRepository, IConferenceRepository conferenceRepository,
    IAuthenticationService authenticationService)
    : IRequestHandler<GetAttendeesInfoListQuery, IQueryResult<GetAttendeesInfoListResponse>>
{
    public async Task<IQueryResult<GetAttendeesInfoListResponse>> Handle(GetAttendeesInfoListQuery request, CancellationToken cancellationToken)
    {
        var conference = await conferenceRepository.GetByIdAsync(request.ConferenceId, cancellationToken);

        if (conference is null)
        {
            return QueryResult.Failure<GetAttendeesInfoListResponse>(new ErrorResult
            {
                ErrorCode = "ConferenceNotFound",
                StatusCode = HttpStatusCode.NotFound
            });
        }
        
        var currentUser = await authenticationService.GetCurrentUser();

        var isParticipant = await attendeeRepository.IsUserAttendeeOfConference(currentUser.Id, request.ConferenceId, cancellationToken);

        if (!conference.IsPublic && !isParticipant)
        {
            return QueryResult.Failure<GetAttendeesInfoListResponse>(new ErrorResult
            {
                ErrorCode = "AccessDenied",
                StatusCode = HttpStatusCode.Forbidden
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
