using Application.Conferences.Models;
using Domain.Models;

namespace Application.Conferences.GetParticipants;

public record GetParticipantsResponse(PagedList<AttendeeDto> Attendees);