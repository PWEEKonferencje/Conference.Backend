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