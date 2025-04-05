using FluentValidation;

namespace Application.Common.Extensions;

public static class FluentValidatorExtension
{
	public static IRuleBuilderOptions<T, string> ValidUrl<T>
		(this IRuleBuilder<T, string> ruleBuilder)
	{
		return ruleBuilder
			.Must(x => string.IsNullOrEmpty(x) || Uri.TryCreate(x, UriKind.Absolute, out _))
			.WithMessage("Invalid URL");
	}
}