using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;

namespace WebApi;

public class GlobalExceptionHandler : IExceptionHandler
{
	public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
	{
		httpContext.Response.ContentType = "application/json";
		httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

		var response = new
		{
			error = new
			{
				message = exception.Message,
				stackTrace = exception.StackTrace
			}
		};

		var result = JsonSerializer.Serialize(response);

		await httpContext.Response.WriteAsync(result);

		return true;
	}
}