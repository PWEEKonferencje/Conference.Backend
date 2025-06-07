using Application.Common.Services;
using Domain.Services.Abstractions;
using Domain.Shared;
using MediatR;

namespace Application.Papers.CreatePaper;

public record CreatePaperCommand(int ConferenceId, string Title, string? Abstract, string? Authors, List<string>? Keywords) : IRequest<ICommandResult<CreatePaperResponse>>;

internal class CreatePaperCommandHandler(IAuthenticationService authenticationService, IPaperService paperService) 
	: IRequestHandler<CreatePaperCommand, ICommandResult<CreatePaperResponse>>
{
	public async Task<ICommandResult<CreatePaperResponse>> Handle(CreatePaperCommand request, CancellationToken cancellationToken)
	{
		var attendee = await authenticationService.GetCurrentAttendee(request.ConferenceId);
		var paper = await paperService.CreatePaperAsync(request.ConferenceId, 
			request.Title, 
			request.Abstract, 
			request.Authors, 
			request.Keywords, 
			attendee, 
			cancellationToken);

		if (!paper.IsSuccess)
			return CommandResult.Failure<CreatePaperResponse>(paper.ErrorResultOptional!);

		return CommandResult.Success(new CreatePaperResponse(paper.Value!.Id));
	}
}