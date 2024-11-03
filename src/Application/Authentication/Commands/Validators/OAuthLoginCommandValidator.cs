using Application.Common.Consts;
using FluentValidation;
using Infrastructure.Authentication;

namespace Application.Authentication.Commands.Validators;

public class OAuthLoginCommandValidator : AbstractValidator<OAuthLoginCommand>
{
	public OAuthLoginCommandValidator(AuthenticationConfiguration authenticationConfiguration)
	{
		RuleFor(x => x.Provider)
			.NotEmpty().WithMessage(ValidationError.NotEmpty)
			.Must(x => authenticationConfiguration.Providers.Contains(x)).WithMessage("Invalid provider");
		RuleFor(x => x.ReturnUrl)
			.NotEmpty();
	}
}