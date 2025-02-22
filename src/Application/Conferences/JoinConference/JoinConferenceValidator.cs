using FluentValidation;

namespace Application.Conferences.JoinConference;

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