using System.Net;
using Application.Affiliations.Models;
using Application.Conferences.Models;
using AutoMapper;
using Domain.Enums;
using Domain.Repositories;
using Domain.Shared;
using MediatR;

namespace Application.Conferences.GetParticipants;

public record GetParticipantsQuery(int ConferenceId, int Page = 1, int PageSize = 20) : IRequest<IQueryResult<GetParticipantsResponse>>;

internal class GetParticipantsQueryHandler(IAttendeeRepository attendeeRepository, IPaperRepository paperRepository,
    IReviewRepository reviewRepository, IConferenceRepository conferenceRepository, IMapper mapper)
    
    : IRequestHandler<GetParticipantsQuery, IQueryResult<GetParticipantsResponse>>
{
    public async Task<IQueryResult<GetParticipantsResponse>> Handle(GetParticipantsQuery request, CancellationToken cancellationToken)
    {
        var conferenceExists = await conferenceRepository
            .ExistAsync(c => c.Id == request.ConferenceId, cancellationToken);

        if (!conferenceExists)
        {
            return QueryResult.Failure<GetParticipantsResponse>(new ErrorResult
            {
                ErrorCode = "ConferenceNotFound",
                StatusCode = HttpStatusCode.NotFound,
            });
        }

        var pagedParticipants = await attendeeRepository.GetParticipantsWithDetails(
            conferenceId: request.ConferenceId,
            page: request.Page,
            pageSize: request.PageSize,
            cancellationToken: cancellationToken);

        var participants = new List<AttendeeDto>();

        foreach (var attendee in pagedParticipants.Items)
        {
            var user = attendee.User;
            var userSnapshot = attendee.UserSnapshot;

            if (user == null || userSnapshot == null)
                continue;

            var papersCount = await paperRepository.CountAsync(
                p => p.CreatorId == attendee.Id,
                cancellationToken);

            var reviewsCount = await reviewRepository.CountAsync(
                r => r.ReviewerId == attendee.Id,
                cancellationToken);

            var affiliation = new AffiliationDto(
                userSnapshot.Workplace,
                userSnapshot.Position,
                userSnapshot.IsPositionAcademic
            );

            participants.Add(new AttendeeDto(
                attendee.Id,
                userSnapshot.Name,
                userSnapshot.Degree,
                user.Country,
                userSnapshot.Position,
                affiliation,
                attendee.Roles.Select(r => r.RoleEnum.ToString()).ToList(),
                papersCount,
                reviewsCount,
                attendee.CreatedAt
            ));
        }

        var response = new GetParticipantsResponse(pagedParticipants.Map(participants));

        return QueryResult.Success(response);
    }
}
