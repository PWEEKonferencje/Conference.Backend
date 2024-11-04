using Domain.Shared;
using Infrastructure.Dictionaries.UniversityNames;
using MediatR;

namespace Application.Dictionaries.Queries;

public record GetUniversityNamesQuery(string Search, int Count) : IRequest<IQueryResult<IReadOnlyList<UniversityNameModel>>>;

internal class GetUniversityNamesQueryHandler(IUniversityNameRepository repository)
	: IRequestHandler<GetUniversityNamesQuery, IQueryResult<IReadOnlyList<UniversityNameModel>>>
{
	public Task<IQueryResult<IReadOnlyList<UniversityNameModel>>> Handle(GetUniversityNamesQuery request, CancellationToken cancellationToken)
	{
		var universities = repository.SearchUniversities(request.Search, request.Count);
		return Task.FromResult(QueryResult.Success(universities));
	}
}