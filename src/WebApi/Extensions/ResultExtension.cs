using Domain.Shared;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Extensions;

public static class ResultExtension
{
	public static IActionResult ToActionResult<T>(this IResult<T> commandResult, int successStatusCode = StatusCodes.Status200OK) where T : class
	{
		if (commandResult.IsSuccess)
		{
			return new ObjectResult(commandResult.Result)
			{
				StatusCode = successStatusCode
			};
		}

		if (commandResult.ErrorResultOptional is null)
		{
			return new ObjectResult(ErrorResult.GenericError)
			{
				StatusCode = 500
			};
		}

		return new ObjectResult(commandResult.ErrorResultOptional)
		{
			StatusCode = (int)commandResult.ErrorResultOptional.StatusCode
		};
	}
}