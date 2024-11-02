using System.Net;

namespace Domain.Shared;

public class ErrorResult
{
	public string? ErrorCode { get; set; }

	public string? ErrorDescription { get; set; }

	public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.InternalServerError;

	public IReadOnlyCollection<Error>? Errors { get; set; }

	public static ErrorResult GenericError = new ErrorResult()
	{
		ErrorDescription = "Unknown error occured"
	};

}