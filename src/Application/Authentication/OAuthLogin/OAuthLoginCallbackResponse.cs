namespace Application.Authentication.OAuthLogin;

public class OAuthLoginCallbackResponse
{
	public string AccessToken { get; set; } = string.Empty;
	public bool IsAccountSetupFinished { get; set; } = false;
	public bool IsEmailProvided { get; set; }
}