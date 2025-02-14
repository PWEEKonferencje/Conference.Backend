using Domain.Models.Dictionary;

namespace Application.Common.Services;

public interface IUniversityNameRepository
{
	IReadOnlyList<UniversityNameModel> SearchUniversities(string search, int count = 10);
	bool IsValidName(string name);
}