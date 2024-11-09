using System.Collections.Immutable;
using System.Reflection;
using System.Text.Json;

namespace Infrastructure.Dictionaries.UniversityNames;

public class UniversityNameRepository : IUniversityNameRepository
{
	private readonly IReadOnlyList<UniversityNameModel> _universityNames;
	private const string ResourceName = $"{nameof(Infrastructure)}.Data.universities.json";
	public UniversityNameRepository()
	{
		var assembly = Assembly.GetExecutingAssembly();
		using var stream = assembly.GetManifestResourceStream(ResourceName)!;
		if (stream == null)
		{
			throw new FileNotFoundException($"Embedded resource '{ResourceName}' not found.");
		}
		using var reader = new StreamReader(stream);
			var jsonData = reader.ReadToEnd();
			_universityNames = JsonSerializer.Deserialize<IReadOnlyList<UniversityNameModel>>(jsonData) 
			                   ?? throw new JsonException("Failed to deserialize university names.");
	}

	public IReadOnlyList<UniversityNameModel> SearchUniversities(string search, int count = 10)
		=> _universityNames
			.Where(x => x.Name.StartsWith(search, StringComparison.OrdinalIgnoreCase))
				.Take(count)
				.ToImmutableList();

	public bool IsValidName(string name)
		=> _universityNames.SingleOrDefault(x => x.Name == name) is not null;
}