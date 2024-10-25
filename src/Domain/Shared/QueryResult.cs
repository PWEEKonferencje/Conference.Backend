namespace Domain.Shared;

public class QueryResult<T>
{
	public ErrorResult? ErrorResultOptional { get; private set; }

	public T Result { get; private set; } = default!;
	
	public bool IsSuccess => ErrorResultOptional == null;
	
	public static QueryResult<T> Success(T result) => new QueryResult<T>
	{
		Result = result
	};
	
	public static QueryResult<T> Failure(ErrorResult errorResult) => new QueryResult<T>
	{
		ErrorResultOptional = errorResult
	};
}