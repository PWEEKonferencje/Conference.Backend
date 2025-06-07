using FluentValidation;

namespace Application.Papers.CreatePaper;

public class CreatePaperValidator : AbstractValidator<CreatePaperCommand>
{
	public CreatePaperValidator()
	{
		RuleFor(x => x.Title).MaximumLength(200);
		RuleFor(x => x.Abstract).MaximumLength(500);
		RuleFor(x => x.Authors).MaximumLength(200);
		RuleForEach(x => x.Keywords).MaximumLength(50);
	}
}