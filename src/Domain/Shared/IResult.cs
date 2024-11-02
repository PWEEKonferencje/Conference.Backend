namespace Domain.Shared;

public interface IResult<T>
{
	public ErrorResult? ErrorResultOptional { get; init; }

	public T Result { get; init; }

	public bool IsSuccess { get; }
}