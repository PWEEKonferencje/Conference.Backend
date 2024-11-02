namespace Domain.Shared;

public static class QueryResult
{
	public static IQueryResult<T> Success<T>(T result) 
		=> new QueryResult<T>
		{
			Result = result,
			ErrorResultOptional = null
		};
	
	public static IQueryResult<T> Failure<T>(ErrorResult errorResult) 
		=> new QueryResult<T>
	{
		Result = default!,
		ErrorResultOptional = errorResult
	};
}

public interface IQueryResult<T> : IResult<T>;

public class QueryResult<T> : IQueryResult<T>
{
	public ErrorResult? ErrorResultOptional { get; init; }

	public T Result { get; init; } = default!;
	
	public bool IsSuccess => ErrorResultOptional == null;
}