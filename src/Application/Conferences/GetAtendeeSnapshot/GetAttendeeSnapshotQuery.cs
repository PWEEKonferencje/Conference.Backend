using Application.Common.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using Domain.Shared;
using Domain.Models.Conference;
using MediatR;
using Application.Conferences.GetAttendeeSnapshot;
using System.Net;
namespace Application.Conferences.GetAttendeeSnapshot;

public record GetAttendeeSnapshotQuery(int ConferenceId, int AttendeeId) : IRequest<IQueryResult<GetAttendeeSnapshotResponse>>;

internal class GetAttendeeSnapshotQueryHandler(IAuthenticationService authenticationService, IAttendeeRepository attendeeRepository, IMapper mapper)
	: IRequestHandler<GetAttendeeSnapshotQuery, IQueryResult<GetAttendeeSnapshotResponse>>
{
	public async Task<IQueryResult<GetAttendeeSnapshotResponse>> Handle(GetAttendeeSnapshotQuery request,CancellationToken cancellationToken)
	{
		var attendee = await attendeeRepository.GetWithUserSnapshotAsync(
			x => x.ConferenceId == request.ConferenceId && x.Id == request.AttendeeId);

		if (attendee is null)
			return QueryResult.Failure<GetAttendeeSnapshotResponse>(new ErrorResult
            {
                ErrorCode = "Attendee SnapShot not found",
                StatusCode = HttpStatusCode.NotFound,
            });

		var userSnapshotModel = mapper.Map<UserSnapshotModel>(attendee.UserSnapshot);
		userSnapshotModel.CreatedAt=attendee.CreatedAt;

		var response = new GetAttendeeSnapshotResponse(userSnapshotModel);

		return QueryResult.Success(response);
	}
}