using FluentValidation;

namespace Application.Conferences.AddRoleToAttendee;

public class AddRoleToAttendeeCommandValidator : AbstractValidator<AddRoleToAttendeeCommand>
{
    public AddRoleToAttendeeCommandValidator()
    {
        RuleFor(x => x.ConferenceId)
            .NotEmpty().WithMessage("ConferenceId is required.");

        RuleFor(x => x.AttendeeId)
            .NotEmpty().WithMessage("AttendeeId is required.");

        RuleFor(x => x.Role).NotNull();
    }
}
