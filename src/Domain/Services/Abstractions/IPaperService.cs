using Domain.Entities;
using Domain.Shared;

namespace Domain.Services.Abstractions;

public interface IPaperService
{
	Task<Result<Paper>> CreatePaperAsync(
		int conferenceId,
		string title,
		string? abstractText,
		string? authors,
		List<string>? keywords,
		Attendee creator,
		CancellationToken cancellationToken = default);
}