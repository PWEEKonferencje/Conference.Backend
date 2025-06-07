using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Domain.Services.Abstractions;
using Domain.Shared;

namespace Domain.Services;

public class PaperService(IPaperRepository paperRepository, 
	IUnitOfWork unitOfWork, 
	IAttendeeRepository attendeeRepository,
	IConferenceRepository conferenceRepository)
	: IPaperService
{
	public async Task<Result<Paper>> CreatePaperAsync(
		int conferenceId, 
		string title, 
		string? abstractText, 
		string? authors, 
		List<string>? keywords,
		Attendee creator,
		CancellationToken cancellationToken = default)
	{
		var errors = new List<Error>();
		
		var conference = await conferenceRepository.GetByIdAsync(conferenceId, cancellationToken);
		if (conference is null)
		{
			errors.Add(new Error("conference", "Conference not found"));
			return Result<Paper>.Failure(errors);
		}
		
		if (!await attendeeRepository.AttendeeHasRoleAsync(creator.Id, AttendeeRoleEnum.Participant))
		{
			errors.Add(new Error("You must be a participant to submit a paper."));
		}
		
		if (conference.IsArticlesDeadlinePassed)
		{
			errors.Add(new Error("The deadline for submitting articles has passed."));
		}
		
		if (errors.Count != 0)
			return Result<Paper>.Failure(errors);
		var paper = Paper.Create(title, abstractText, authors, keywords, creator, conference);
		paperRepository.Add(paper, cancellationToken);
		await unitOfWork.SaveChangesAsync(cancellationToken);
		return Result<Paper>.Success(paper);
	}
}