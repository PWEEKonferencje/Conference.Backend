using Domain.Enums;
using Domain.Shared;

namespace Domain.Entities;

public class Invitation
{
	public Guid Id { get; set; }
	public InvitationType Type { get; set; }
	public bool IsDeleted { get; set; } = false;
	
	public int ConferenceId { get; set; }
	public Conference Conference { get; set; } = null!;

	public static Result<Invitation> Create(Conference conference, InvitationType type)
	{
		if (conference.RegistrationDeadline < DateTime.UtcNow)
		{
			return Result<Invitation>.Failure([new Error(errorMessage: "Registration deadline already passed")]);	
		}
		
		return Result<Invitation>.Success(new Invitation
		{
			Type = type,
			ConferenceId = conference.Id
		});
	}
}