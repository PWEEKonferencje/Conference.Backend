using FluentValidation;

namespace Application.Affiliations.CreateAffiliation;

public class CreateAffiliationValidator : AbstractValidator<CreateAffiliationModel>
{
	public CreateAffiliationValidator()
	{
		RuleFor(x => x.Workplace)
			.NotEmpty()
			.MaximumLength(150);
		RuleFor(x => x.Position)
			.NotEmpty()
			.MaximumLength(100);
	}
}