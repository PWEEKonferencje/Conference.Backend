using Application.Common.Consts;
using Domain.Entities.Identity;
using FluentValidation;
using Infrastructure.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Profiles.Commands.Validators;

public class SetProfileOrcidCommandValidator : AbstractValidator<SetProfileOrcidCommand>
{
	public SetProfileOrcidCommandValidator(UserManager<UserAccount> userManager, IUserContextService userContextService)
	{
		RuleFor(x => x.OrcidId)
			.NotEmpty().WithMessage(ValidationError.NotEmpty)
			.Matches(@"\d{4}-\d{4}-\d{4}-\d{4}").WithMessage(ValidationError.InvalidFormat)
			.MustAsync(async (orcidId, cancellation) =>
				await userManager.Users.CountAsync(y => y.OrcidId == orcidId, cancellation) == 0)
			.WithMessage(ValidationError.NotUnique)
			.MustAsync(async (_, _) =>
				(await userManager.GetUserAsync(userContextService.User!))!.OrcidId is null)
			.WithMessage(ValidationError.AlreadySet);
	}
}