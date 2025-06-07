using Application.Affiliations.CreateAffiliation;
using Application.Common.Consts;
using FluentValidation;
using Maddalena;

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

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Email is not valid")
            .MaximumLength(50).WithMessage(ValidationError.MaximumLength(50));
        
        RuleFor(x => x.Phone)
            .MaximumLength(20).WithMessage(ValidationError.MaximumLength(20));

        RuleFor(x => x.ResearchInterest)
            .MaximumLength(200).WithMessage(ValidationError.MaximumLength(200));
        
        RuleFor(x => x.Country)
				.Must(x => x == null || Country.All.Any(r => r.CommonName == x)).WithMessage("Country is not valid");
    }
}