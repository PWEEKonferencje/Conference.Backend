using FluentValidation;
using MediatR;

namespace Application.Common;

public class ValidationBehaviour<TRequest, TResponse>
(IEnumerable<IValidator<TRequest>> validators)
	: IPipelineBehavior<TRequest, TResponse> 
	where TRequest : notnull
{
	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		var context = new ValidationContext<TRequest>(request);
		
		var validationFailures
			= await Task.WhenAll(
				validators
					.Select(x =>
						x.ValidateAsync(context, cancellationToken)
						));
		
		var errors
			= validationFailures
				.Where(x => !x.IsValid)
				.SelectMany(x => x.Errors)
				.ToList();
		if (errors.Count != 0)
		{
			throw new ValidationException(errors);
		}
		
		var result = await next();
		return result;
	}
}