namespace Domain.Shared;

public class Error
{
	public Error(){}
	
	public Error(string errorMessage, string? errorField = null, string? errorCode = null)
	{
		ErrorMessage = errorMessage;
		ErrorField = errorField;
		ErrorCode = errorCode;
	}
	
	public string? ErrorCode { get; set; }

	public string? ErrorField { get; set; }

	public string? ErrorMessage { get; set; }
}