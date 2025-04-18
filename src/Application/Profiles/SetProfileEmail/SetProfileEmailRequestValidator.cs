using Application.Common.Consts;
using Application.Common.Services;
using Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Profiles.SetProfileEmail;

public class SetProfileEmailRequestValidator : AbstractValidator<SetProfileEmailCommand>
{
	public SetProfileEmailRequestValidator(UserManager<Identity> userManager, IUserContextService userContextService)
	{
		RuleFor(x => x.Email)
			.NotEmpty().WithMessage(ValidationError.NotEmpty)
			.EmailAddress().WithMessage(ValidationError.InvalidFormat)
			.MustAsync(async (_, _)
				=> (await userManager.GetUserAsync(userContextService.User!))!.Email is null)
			.WithMessage(ValidationError.AlreadySet)
			.MustAsync(async (email, cancellation)
				=> !await userManager.Users.Where(y => y.Email == email).AnyAsync(cancellation))
			.WithMessage(ValidationError.NotUnique);
	}
}