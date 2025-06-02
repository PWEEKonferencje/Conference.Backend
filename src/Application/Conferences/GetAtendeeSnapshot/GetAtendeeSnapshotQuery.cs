using Application.Common.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using Domain.Shared;
using Domain.Models.Conference;
using MediatR;
using Application.Conferences.GetAtendeeSnapshot;
using System.Net;
namespace Application.Conferences.GetAtendeeSnapshot;

public record GetAtendeeSnapshotQuery(int ConferenceId, int AtendeeId) : IRequest<IQueryResult<GetAtendeeSnapshotResponse>>;

internal class GetAtendeeSnapshotQueryHandler(IAuthenticationService authenticationService, IAttendeeRepository attendeeRepository, IMapper mapper)
	: IRequestHandler<GetAtendeeSnapshotQuery, IQueryResult<GetAtendeeSnapshotResponse>>
{
	public async Task<IQueryResult<GetAtendeeSnapshotResponse>> Handle(GetAtendeeSnapshotQuery request,CancellationToken cancellationToken)
	{
		var user = await authenticationService.GetCurrentUser();
		if (user is null || user.IsProfileSetUp is false)
			return QueryResult.Failure<GetAtendeeSnapshotResponse>(ErrorResult.AuthorizationError);

		var attendee = await attendeeRepository.GetWithUserSnapshotAsync(
			x => x.ConferenceId == request.ConferenceId && x.Id == request.AtendeeId);

		if (attendee is null)
			return QueryResult.Failure<GetAtendeeSnapshotResponse>(new ErrorResult
            {
                ErrorCode = "Attendee SnapShot not found",
                StatusCode = HttpStatusCode.NotFound,
            });

		var userSnapshotModel = mapper.Map<UserSnapshotModel>(attendee.UserSnapshot);
		userSnapshotModel.CreatedAt=attendee.CreatedAt;

		var response = new GetAtendeeSnapshotResponse(userSnapshotModel);

		return QueryResult.Success(response);
	}
}