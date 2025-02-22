using FluentValidation;

namespace Application.Affiliations.CreateAffiliation;

public class CreateAffiliationCommandValidator : AbstractValidator<CreateAffiliationCommand>
{
    public CreateAffiliationCommandValidator(IValidator<CreateAffiliationModel> validator)
	{
		RuleFor(x => x.Affiliation).SetValidator(validator);
	}
}