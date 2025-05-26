using FluentValidation;

namespace Application.Invitations.CreateInvitation;

public class CreateInvitationValidator : AbstractValidator<CreateInvitationCommand>
{
	public CreateInvitationValidator()
	{
		RuleFor(x => x.ConferenceId).NotEmpty().GreaterThan(0);
		RuleFor(x => x.InvitationType).NotNull();
	}
}