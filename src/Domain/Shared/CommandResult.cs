namespace Domain.Shared;

public class CommandResult<T>
{
	public ErrorResult? ErrorResultOptional { get; set; }

	public T Result { get; set; } = default!;
	
	public bool IsSuccess => ErrorResultOptional == null;
	
	public static CommandResult<T> Success(T result) => new CommandResult<T>
	{
		Result = result,
		ErrorResultOptional = null
	};
	
	public static CommandResult<T> Failure(ErrorResult errorResult) => new CommandResult<T>
	{
		Result = default!,
		ErrorResultOptional = errorResult
	};
}