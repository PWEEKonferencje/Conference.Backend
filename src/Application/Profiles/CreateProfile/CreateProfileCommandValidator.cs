using Application.Affiliations.CreateAffiliation;
using Application.Common.Consts;
using FluentValidation;

namespace Application.Profiles.CreateProfile;

public class CreateProfileCommandValidator : AbstractValidator<CreateProfileCommand>
{
    public CreateProfileCommandValidator(IValidator<CreateAffiliationModel> affiliationValidator)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(ValidationError.NotEmpty)
            .MaximumLength(50).WithMessage(ValidationError.MaximumLength(50));

        RuleFor(x => x.Surname)
            .NotEmpty().WithMessage(ValidationError.NotEmpty)
            .MaximumLength(50).WithMessage(ValidationError.MaximumLength(50));

        RuleFor(x => x.Affiliations)
            .ForEach(x => x.SetValidator(affiliationValidator));

        RuleFor(x => x.Degree)
            .MaximumLength(40).WithMessage(ValidationError.MaximumLength(40));
    }
}