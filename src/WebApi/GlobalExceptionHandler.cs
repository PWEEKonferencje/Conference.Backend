using System.Security.Authentication;
using System.Text.Json;
using Application.Common.Exceptions;
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
		ErrorResult? errorResult = null;
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
		else if (exception is AuthenticationException authException)
		{
			httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
		}
		else if (exception is ProfileNotSetUpException profileNotSetUpException)
		{
			httpContext.Response.StatusCode = StatusCodes.Status428PreconditionRequired;
			errorResult = new ErrorResult
			{
				ErrorCode = "ProfileNotSetUp",
				ErrorDescription = "User profile is not set up, but it is required for this operation"
			};
		}
		else
		{
			httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
			errorResult = ErrorResult.GenericError;
			logger.LogError("Request returned 500. An error occurred: {Error}", exception.Message);
		}

		if (errorResult is not null)
		{
			var result = JsonSerializer.Serialize(errorResult);
			await httpContext.Response.WriteAsync(result, cancellationToken: cancellationToken);
		}
		
		return true;
	}
}