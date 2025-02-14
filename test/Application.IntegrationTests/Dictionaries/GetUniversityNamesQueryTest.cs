using Application.Dictionaries.Queries;
using FluentAssertions;
using IntegrationTests.Base;

namespace IntegrationTests.Dictionaries;

public class GetUniversityNamesQueryTest(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
	[Fact]
	public async Task ShouldReturnUniversityName()
	{
		// Arrange
		var query = new GetUniversityNamesQuery("war", 10);

		// Act
		var result = await Mediator.Send(query);

		// Assert
		result.Should().NotBeNull();
		result.ErrorResultOptional.Should().BeNull();
		result.IsSuccess.Should().BeTrue();
		result.Result.Should().HaveCount(8);
		result.Result.Should().Contain(x => x.Name == "Warsaw University of Technology");
	}
}