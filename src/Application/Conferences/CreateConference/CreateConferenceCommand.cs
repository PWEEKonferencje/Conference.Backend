using Application.Common.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Domain.Shared;
using MediatR;

namespace Application.Conferences.CreateConference;

public record CreateConferenceCommand : IRequest<ICommandResult<CreateConferenceResponse>>
{
	public string Name { get; init; } = default!;
	public string Description { get; set; } = default!;
	public DateTime StartDate { get; set; }
	public DateTime EndDate { get; set; }
	public DateTime RegistrationDeadline { get; set; }
	public DateTime ArticlesDeadline { get; set; }
	public Guid UserAffiliationId { get; set; } = default!;
}

internal class CreateConferenceCommandHandler(IAuthenticationService authenticationService, IMapper mapper, 
	IAffiliationRepository affiliationRepository, IConferenceRepository conferenceRepository, IUnitOfWork unitOfWork) 
	: IRequestHandler<CreateConferenceCommand, ICommandResult<CreateConferenceResponse>>
{
	public async Task<ICommandResult<CreateConferenceResponse>> Handle(CreateConferenceCommand request, CancellationToken cancellationToken)
	{
		var user = await authenticationService.GetCurrentUser();
		if (user is null || user.IsProfileSetUp is false)
			return CommandResult.Failure<CreateConferenceResponse>(ErrorResult.AuthorizationError);
		var affiliation = await affiliationRepository.GetFirstAsync(x => x.Id == request.UserAffiliationId, cancellationToken);
		
		if (affiliation is null)
			return CommandResult.Failure<CreateConferenceResponse>(
				ErrorResult.DomainError([new Error("Affiliation Not Found", nameof(request.UserAffiliationId))]));
		
		var conference = mapper.Map<Conference>(request);
		var attendee = Attendee.Create(AttendeeRoleEnum.Organizer, user, affiliation);
		conference.Attendees.Add(attendee);
		
		conferenceRepository.Add(conference, cancellationToken);
		await unitOfWork.SaveChangesAsync(cancellationToken);
		
		return CommandResult.Success(new CreateConferenceResponse { Id = conference.Id });
	}
}