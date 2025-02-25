using Application.Common.Consts;
using FluentValidation;
using Maddalena;

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

		RuleFor(x => x.Address)
			.NotNull().WithMessage(ValidationError.NotEmpty);

		When(x => x.Address is not null, () =>
		{
			RuleFor(x => x.Address.PlaceName)
				.NotEmpty().WithMessage(ValidationError.NotEmpty)
				.MaximumLength(200).WithMessage(ValidationError.MaximumLength(200));

			RuleFor(x => x.Address.AddressLine1)
				.NotEmpty().WithMessage(ValidationError.NotEmpty)
				.MaximumLength(200).WithMessage(ValidationError.MaximumLength(200));

			RuleFor(x => x.Address.AddressLine2)
				.MaximumLength(200).WithMessage(ValidationError.MaximumLength(200));

			RuleFor(x => x.Address.City)
				.NotEmpty().WithMessage(ValidationError.NotEmpty)
				.MaximumLength(50).WithMessage(ValidationError.MaximumLength(50));

			RuleFor(x => x.Address.ZipCode)
				.NotEmpty().WithMessage(ValidationError.NotEmpty)
				.MaximumLength(15).WithMessage(ValidationError.MaximumLength(15));

			RuleFor(x => x.Address.Country)
				.NotEmpty().WithMessage(ValidationError.NotEmpty)
				.Must(x => Country.All.Any(r => r.CommonName == x)).WithMessage("Country is not valid");
		});
	}
}