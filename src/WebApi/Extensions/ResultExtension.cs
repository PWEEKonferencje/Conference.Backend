using Domain.Shared;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Extensions;

public static class ResultExtension
{
	public static IActionResult ToActionResult<T>(this ICommandResult<T> commandResult, int successStatusCode = StatusCodes.Status200OK)
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
	
	public static IActionResult ToActionResult<T>(this IQueryResult<T> commandResult, int successStatusCode = StatusCodes.Status200OK)
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