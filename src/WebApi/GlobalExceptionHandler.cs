using System.Text.Json;
using Domain.Shared;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

namespace WebApi;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
	public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,  CancellationToken cancellationToken)
	{
		httpContext.Response.Clear();
		httpContext.Response.ContentType = "application/json";
		ErrorResult errorResult;
		if (exception is ValidationException validationException)
		{
			httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
			errorResult = new ErrorResult
			{
				ErrorCode = "ValidationError",
				Errors =
					validationException
						.Errors
						.Select(x => new Error
						{
							ErrorField = x.PropertyName,
							ErrorMessage = x.ErrorMessage,
							ErrorCode = x.ErrorCode
						})
						.ToList(),
				ErrorDescription = "Validation Error Occured"
			};
			logger.LogInformation("Request returned 400. Validation error occured: {Error}", errorResult);
		}
		else
		{
			httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
			errorResult = ErrorResult.GenericError;
			logger.LogError("Request returned 500. An error occurred: {Error}", exception.Message);
		}
		
		var result = JsonSerializer.Serialize(errorResult);

		await httpContext.Response.WriteAsync(result, cancellationToken: cancellationToken);
		
		return true;
	}
}