using Application.Common.Consts;
using Application.Common.Services;
using FluentValidation;

namespace Application.Profiles.SetProfileOrcid;

public class SetProfileOrcidCommandValidator : AbstractValidator<SetProfileOrcidCommand>
{
	public SetProfileOrcidCommandValidator(IAuthenticationService userManager, IUserContextService userContextService)
	{
		RuleFor(x => x.OrcidId)
			.NotEmpty().WithMessage(ValidationError.NotEmpty)
			.Matches(@"\d{16}").WithMessage(ValidationError.InvalidFormat)
			.MustAsync(async (_, _) =>
				(await userManager.GetCurrentIdentity())?.UserProfileId is null)
			.WithMessage(ValidationError.AlreadySet);
	}
}