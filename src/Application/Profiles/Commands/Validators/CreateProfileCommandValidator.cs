using Application.Common.Consts;
using FluentValidation;
using Infrastructure.Dictionaries.UniversityNames;

namespace Application.Profiles.Commands.Validators;

public class CreateProfileCommandValidator : AbstractValidator<CreateProfileCommand>
{
    public CreateProfileCommandValidator(IUniversityNameRepository universityNameRepository)
    {
        RuleFor(x => x.CreateProfileRequest.Name)
            .NotEmpty().WithMessage(ValidationError.NotEmpty);

        RuleFor(x => x.CreateProfileRequest.Surname)
            .NotEmpty().WithMessage(ValidationError.NotEmpty);

        RuleFor(x => x.CreateProfileRequest.University)
            .Must(universityName => universityName == null || universityNameRepository.IsValidName(universityName))
            .WithMessage(ValidationError.NotValid);
        
    }
}