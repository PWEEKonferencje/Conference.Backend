using Application.Profiles.CreateProfile;
using Domain.Entities;
using FluentAssertions;
using IntegrationTests.Base;
using IntegrationTests.Base.Helpers;

namespace IntegrationTests.Profiles;

public class CreateProfileCommandHandlerTest(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
	[Fact]
	public async Task ShouldCreateProfile()
	{
		// Arrange
		var command = new CreateProfileCommand{
			Name = Faker.Name.FirstName(),
			Surname = Faker.Name.LastName(),
			Degree = "PhD"
		};
		new IdentityBuilder()
			.WithId("0")
			.WithUserName(Consts.UserName)
			.WithEmail(Consts.Email)
			.AddToDatabase(DbContext)
			.SetInHttpContextAccessor(HttpContextAccessor)
			.Build();

		// Act
		var result = await Mediator.Send(command);

		// Assert
		result.Should().NotBeNull();
		result.IsSuccess.Should().BeTrue();
		result.ErrorResultOptional.Should().BeNull();
		result.Result.Should().NotBeNull();
	}
	
	[Fact]
	public async Task ShouldReturnErrorWhenProfileAlreadyExists()
	{
		// Arrange
		var command = new CreateProfileCommand{
			Name = Faker.Name.FirstName(),
			Surname = Faker.Name.LastName(),
			Degree = "PhD"
		};
		new IdentityBuilder()
			.WithId("0")
			.WithUserName(Consts.UserName)
			.WithEmail(Consts.Email)
			.WithProfile(new User
			{
				Name = Faker.Name.FirstName(),
				Surname = Faker.Name.LastName(),
				Degree = "PhD"
			})
			.AddToDatabase(DbContext)
			.SetInHttpContextAccessor(HttpContextAccessor)
			.Build();
		await Mediator.Send(command);

		// Act
		var result = await Mediator.Send(command);

		// Assert
		result.Should().NotBeNull();
		result.IsSuccess.Should().BeFalse();
		result.ErrorResultOptional.Should().NotBeNull();
		result.ErrorResultOptional!.ErrorCode.Should().Be("ProfileAlreadyExists");
	}
}