using System.Text.Json;
using Domain.Shared;
using Microsoft.AspNetCore.Diagnostics;

namespace WebApi;

public class GlobalExceptionHandler : IExceptionHandler
{
	public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
	{
		httpContext.Response.ContentType = "application/json";
		httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

		var result = JsonSerializer.Serialize(ErrorResult.GenericError);

		await httpContext.Response.WriteAsync(result);

		return true;
	}
}