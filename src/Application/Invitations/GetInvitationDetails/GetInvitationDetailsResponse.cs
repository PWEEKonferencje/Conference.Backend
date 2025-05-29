using Application.Conferences.Models;

namespace Application.Invitations.GetInvitationDetails;

public record GetInvitationDetailsResponse(ConferenceDetailsDto ConferenceDetails, bool IsRegistrationDeadlinePassed, List<TrackDto> Tracks);