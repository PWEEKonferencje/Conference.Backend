using FluentValidation;

namespace Application.Attendees.JoinConference;

public class JoinConferenceValidator : AbstractValidator<JoinConferenceCommand>
{
    public  JoinConferenceValidator()
    {
        RuleFor(x => x.ConferenceId)
            .NotEmpty();
        RuleFor(x => x.AffiliationId)
            .NotEmpty();
    }
}