using FluentValidation;

namespace Application.Common.Extensions;

public static class FluentValidatorExtension
{
	public static IRuleBuilderOptions<T, string> ValidUrl<T>
		(this IRuleBuilder<T, string> ruleBuilder)
	{
		return ruleBuilder
			.Must(x => string.IsNullOrWhiteSpace(x) || Uri.TryCreate(x, UriKind.RelativeOrAbsolute, out _))
			.WithMessage("Invalid URL");
	}
}