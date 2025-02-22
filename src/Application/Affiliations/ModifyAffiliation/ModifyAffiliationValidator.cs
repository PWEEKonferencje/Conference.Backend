using FluentValidation;

namespace Application.Affiliations.ModifyAffiliation;

public class ModifyAffiliationValidator : AbstractValidator<ModifyAffiliationCommand>
{
	public ModifyAffiliationValidator()
	{
		RuleFor(x => x.affiliation.Workplace)
            .NotEmpty()
            .MaximumLength(150);
        RuleFor(x => x.affiliation.Position)
            .NotEmpty()
            .MaximumLength(100);
        RuleFor(x => x.affiliation.Description)
            .MaximumLength(500);
	}
}