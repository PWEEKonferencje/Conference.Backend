namespace Application.Common.Consts;

public static class ValidationError
{
	public const string NotEmpty = "Field cannot be empty";
	public const string AuthorizationFailed = "AuthorizationError";
	public const string NotValid = "Field is not valid";
	public const string InvalidFormat = "Field has invalid format";
	public const string NotUnique = "Field is not unique";
	public const string AlreadySet = "Field is already set";
	public const string FutureData = "Date must be future date";
	public static string MaximumLength(int length) => $"Field cannot be longer than {length} characters";
}