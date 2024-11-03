using System.Net;
using Domain.Shared;
using FluentValidation.Results;

namespace Application.Common.Extensions;

public static class ValidatorExtensions
{
	public static ErrorResult? ToErrorResult(this ValidationResult? validationResult)
	{
		if (validationResult is null || validationResult.IsValid)
			return null;
		return new ErrorResult
		{
			StatusCode = HttpStatusCode.BadRequest,
			ErrorDescription = "Validation failed",
			Errors = validationResult.Errors.Select(x => new Error
			{
				ErrorField = x.PropertyName,
				ErrorMessage = x.ErrorMessage
			}).ToList()
		};
	}
}