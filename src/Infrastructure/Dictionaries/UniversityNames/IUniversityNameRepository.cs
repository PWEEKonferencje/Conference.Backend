namespace Infrastructure.Dictionaries.UniversityNames;

public interface IUniversityNameRepository
{
	IReadOnlyList<UniversityNameModel> SearchUniversities(string search, int count = 10);
	bool IsValidName(string name);
}