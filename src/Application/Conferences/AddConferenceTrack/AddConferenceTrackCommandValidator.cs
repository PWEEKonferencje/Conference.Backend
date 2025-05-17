using FluentValidation;

namespace Application.Conferences.AddConferenceTrack;

public class AddConferenceTrackCommandValidator : AbstractValidator<AddConferenceTrackCommand>
{
    public AddConferenceTrackCommandValidator()
    {
        RuleFor(x => x.ConferenceId)
            .NotEmpty().WithMessage("ConferenceId is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Track name is required.")
            .MaximumLength(150).WithMessage("Track name cannot exceed 150 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");
    }
}