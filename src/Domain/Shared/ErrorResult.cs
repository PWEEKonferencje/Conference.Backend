using System.Net;
using System.Text.Json.Serialization;

namespace Domain.Shared;

public class ErrorResult
{
	public string? ErrorCode { get; set; }

	public string? ErrorDescription { get; set; }

	[JsonIgnore]
	public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.InternalServerError;

	public IReadOnlyCollection<Error>? Errors { get; set; }

	public static ErrorResult GenericError = new ErrorResult()
	{
		ErrorDescription = "Unknown error occured"
	};

}