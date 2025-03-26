using Application.Common.Consts;
using Application.Common.Services;
using FluentValidation;

namespace Application.Profiles.SetProfileOrcid;

public class SetProfileOrcidCommandValidator : AbstractValidator<SetProfileOrcidCommand>
{
	public SetProfileOrcidCommandValidator(IAuthenticationService userManager, IUserContextService userContextService)
	{
		RuleFor(x => x.OrcidId)
			.NotEmpty().WithMessage(ValidationError.NotEmpty)
			.Matches(@"\d{15}[\dX]").WithMessage(ValidationError.InvalidFormat)
			.Must(IsValidOrcid).WithMessage("Invalid ORCID checksum")
			.MustAsync(async (_, _) =>
				(await userManager.GetCurrentIdentity())?.UserProfileId is null)
			.WithMessage(ValidationError.AlreadySet);
	}
	
	private bool IsValidOrcid(string orcid)
	{
		if (string.IsNullOrWhiteSpace(orcid) || orcid.Length != 16)
			return false;

		if (!TryCalculateChecksumBase(orcid, out int checksumBase))
			return false;

		char expectedCheckDigit = CalculateCheckDigit(checksumBase);
		return orcid[15] == expectedCheckDigit;
	}

	private bool TryCalculateChecksumBase(string orcid, out int checksumBase)
	{
		checksumBase = 0;
		string first15Digits = orcid.Substring(0, 15);
		foreach (char digit in first15Digits)
		{
			if (!char.IsDigit(digit))
				return false;
			checksumBase = (checksumBase + (digit - '0')) * 2;
		}
		return true;
	}

	private char CalculateCheckDigit(int checksumBase)
	{
		int remainder = checksumBase % 11;
		int checkValue = (12 - remainder) % 11;
		return checkValue == 10 ? 'X' : (char)('0' + checkValue);
	}
}