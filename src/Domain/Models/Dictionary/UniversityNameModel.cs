using System.Text.Json.Serialization;

namespace Domain.Models.Dictionary;

public record UniversityNameModel
{
	[JsonPropertyName("name")]
	public string Name { get; init; } = default!;

	[JsonPropertyName("country")]
	public string Country { get; init; } = default!;

	[JsonPropertyName("web_page")]
	public string WebPage { get; init; } = default!;
}