using Application.Common.Consts;
using FluentValidation;

namespace Application.Conferences.CreateConference;

public class CreateConferenceCommandValidator : AbstractValidator<CreateConferenceCommand>
{
	public CreateConferenceCommandValidator()
	{
		var date = DateTime.Now;
		
		RuleFor(x => x.Name)
			.NotEmpty().WithMessage(ValidationError.NotEmpty)
			.MaximumLength(150).WithMessage(ValidationError.MaximumLength(150));

		RuleFor(x => x.Description)
			.MaximumLength(1000).WithMessage(ValidationError.MaximumLength(1000));

		RuleFor(x => x.StartDate)
			.NotEmpty().WithMessage(ValidationError.NotEmpty)
			.GreaterThan(date).WithMessage(ValidationError.FutureData);
		
		RuleFor(x => x.EndDate)
			.NotEmpty().WithMessage(ValidationError.NotEmpty)
			.GreaterThan(x => x.StartDate).WithMessage("Date must be after the start date")
			.GreaterThan(date).WithMessage(ValidationError.FutureData);

		RuleFor(x => x.RegistrationDeadline)
			.NotEmpty().WithMessage(ValidationError.NotEmpty)
			.LessThanOrEqualTo(x => x.StartDate).WithMessage("Date must be before the start date")
			.GreaterThan(date).WithMessage(ValidationError.FutureData);
		
		RuleFor(x => x.ArticlesDeadline)
			.NotEmpty().WithMessage(ValidationError.NotEmpty)
			.LessThanOrEqualTo(x => x.StartDate).WithMessage("Date must be before the start date")
			.GreaterThan(date).WithMessage(ValidationError.FutureData);

		RuleFor(x => x.UserAffiliationId)
			.NotEmpty().WithMessage(ValidationError.NotEmpty);
	}
}