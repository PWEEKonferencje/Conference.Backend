namespace Domain.Shared;

public class Result
{
	public ErrorResult? ErrorResultOptional { get; init; }
	public bool IsSuccess => ErrorResultOptional is null;
	
	public static Result Success() => new();
	public static Result Failure(List<Error> errors) 
		=> new() {ErrorResultOptional = ErrorResult.DomainError(errors)};
	public static Result Failure(ErrorResult errorResult) 
		=> new() {ErrorResultOptional = errorResult};
}

public class Result<T>
{
	public T? Value { get; init; }
	public ErrorResult? ErrorResultOptional { get; init; }
	public bool IsSuccess => ErrorResultOptional is null;
	
	public static Result<T> Success(T value) => new Result<T> {Value = value};
	public static Result<T> Failure(List<Error> errors) 
		=> new() {ErrorResultOptional = ErrorResult.DomainError(errors)};
	public static Result<T> Failure(ErrorResult errorResult) 
		=> new() {ErrorResultOptional = errorResult};
}