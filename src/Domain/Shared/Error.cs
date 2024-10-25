namespace Domain.Shared;

public class Error
{
	public string ErrorCode { get; set; } = string.Empty;

	public string? ErrorField { get; set; }

	public string? ErrorMessage { get; set; }
}