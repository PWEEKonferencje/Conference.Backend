using System.Net;
using System.Text.Json.Serialization;

namespace Application.Authentication.OAuthLogin;

public class OAuthLoginCallbackResponse
{
	public OAuthLoginCallbackResponse(string accessToken, bool isAccountSetupFinished = false, 
		bool isEmailProvided = false, string? redirectUrl = null)
	{
		AccessToken = accessToken;
		IsAccountSetupFinished = isAccountSetupFinished;
		IsEmailProvided = isEmailProvided;

		if (string.IsNullOrWhiteSpace(redirectUrl)) return;
		try
		{
			var builder = new UriBuilder(redirectUrl)
			{
				Fragment = $"token={accessToken}",
				Query = $"isAccountSetupFinished={isAccountSetupFinished}&isEmailProvided={isEmailProvided}"
			};
			if (!string.IsNullOrEmpty(builder.Query))
			{
				builder.Query += $"&isAccountSetupFinished={isAccountSetupFinished}&isEmailProvided={isEmailProvided}";
			}
			else
			{
				builder.Query = $"isAccountSetupFinished={isAccountSetupFinished}&isEmailProvided={isEmailProvided}";
			}
			RedirectUrl = builder.Uri;
		}
		catch (UriFormatException ex)
		{
			return;
		}
	}
	public string AccessToken { get; set; }
	public bool IsAccountSetupFinished { get; set; } = false;
	public bool IsEmailProvided { get; set; }

	[JsonIgnore] public Uri? RedirectUrl { get; set; } = null;
}