namespace Application.Common.Configuration;

public class AuthenticationConfiguration
{
	public string JwtKey { get; init; } = default!;
	
	public int JwtExpireTimeDays { get; init; } = 7;

	public string JwtIssuer { get; init; } = default!;

	public string JwtAudience { get; init; } = default!;

	public IReadOnlyList<string> Providers { get; init; } = default!;
}