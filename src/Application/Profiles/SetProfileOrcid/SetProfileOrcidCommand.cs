using Application.Common.Services;
using Domain.Entities;
using Domain.Repositories;
using Domain.Shared;
using MediatR;

namespace Application.Profiles.SetProfileOrcid;

public record SetProfileOrcidCommand(string OrcidId) : IRequest<ICommandResult<SetProfileOrcidResponse>>;

internal class SetProfileOrcidCommandHandler(IAuthenticationService authenticationService,
	IUnitOfWork unitOfWork, IProfileRepository profileRepository)
	: IRequestHandler<SetProfileOrcidCommand, ICommandResult<SetProfileOrcidResponse>>
{
	public async Task<ICommandResult<SetProfileOrcidResponse>> Handle(SetProfileOrcidCommand request, CancellationToken cancellationToken)
	{
		var account = await authenticationService.GetCurrentIdentity();
		if (account is null)
			return CommandResult.Failure<SetProfileOrcidResponse>(ErrorResult.AuthorizationError);

		var profile = await profileRepository.GetFirstAsync(x => x.Id == account.UserProfileId, cancellationToken);
		if (profile?.OrcidId is not null)
			return CommandResult.Failure<SetProfileOrcidResponse>
				(ErrorResult.DomainError([new Error("User already has profile with Orcid Id set")]));
		
		if (await profileRepository.ExistAsync(x => x.OrcidId == request.OrcidId, cancellationToken))
		{
			// send email to identity connected to profile that has that orcid set
			return CommandResult.Success(new SetProfileOrcidResponse{ProfileAlreadyExists = true});
		}

		if (account.UserProfileId is not null && profile is not null)
		{
			profile.OrcidId = request.OrcidId;
		}
		else
		{
			account.UserProfile = new User
			{
				OrcidId = request.OrcidId
			};
		}
		await unitOfWork.SaveChangesAsync(cancellationToken);
		
		return CommandResult.Success(new SetProfileOrcidResponse());
	}
}