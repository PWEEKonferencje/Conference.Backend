namespace Domain.Models.Authentication;

public class LoginResponse
{
	public string AccessToken { get; set; } = string.Empty;
	public bool IsAccountSetupFinished { get; set; } = false;
}