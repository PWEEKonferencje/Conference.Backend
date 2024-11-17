using Application.Profiles.Commands;
using Domain.Entities;
using Domain.Models.Profile;
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
		var command = new CreateProfileCommand(
			new CreateProfileRequest
			{
				Name = Faker.Name.FirstName(),
				Surname = Faker.Name.LastName(),
				University = "Warsaw University of Technology",
				Degree = "PhD"
			}
		);
		new UserAccountBuilder()
			.WithId("0")
			.WithUserName(Consts.UserName)
			.WithEmail(Consts.Email)
			.WithOrcid(Consts.Orcid)
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
		result.Result.Id.Should().NotBe(0);
		result.Result.Name.Should().Be(command.CreateProfileRequest.Name);
		result.Result.Surname.Should().Be(command.CreateProfileRequest.Surname);
		result.Result.University.Should().Be(command.CreateProfileRequest.University);
		result.Result.Degree.Should().Be(command.CreateProfileRequest.Degree);
	}
	
	[Fact]
	public async Task ShouldReturnErrorWhenProfileAlreadyExists()
	{
		// Arrange
		var command = new CreateProfileCommand(
			new CreateProfileRequest
			{
				Name = Faker.Name.FirstName(),
				Surname = Faker.Name.LastName(),
				University = "Warsaw University of Technology",
				Degree = "PhD"
			}
		);
		new UserAccountBuilder()
			.WithId("0")
			.WithUserName(Consts.UserName)
			.WithEmail(Consts.Email)
			.WithOrcid(Consts.Orcid)
			.WithProfile(new UserProfile
			{
				Name = Faker.Name.FirstName(),
				Surname = Faker.Name.LastName(),
				University = "Warsaw University of Technology",
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
	
	[Fact]
	public async Task ShouldThrowValidationExceptionWhenUniversityNameIsInvalid()
	{
		// Arrange
		var command = new CreateProfileCommand(
			new CreateProfileRequest
			{
				Name = Faker.Name.FirstName(),
				Surname = Faker.Name.LastName(),
				University = "Invalid University",
				Degree = "PhD"
			}
		);
		new UserAccountBuilder()
			.WithId("0")
			.WithUserName(Consts.UserName)
			.WithEmail(Consts.Email)
			.WithOrcid(Consts.Orcid)
			.AddToDatabase(DbContext)
			.SetInHttpContextAccessor(HttpContextAccessor)
			.Build();

		// Act
		var action = async () => await Mediator.Send(command);

        // Assert
        var exception = await action.Should().ThrowAsync<FluentValidation.ValidationException>();
        exception.Which.Errors.Should().HaveCount(1);
        exception.Which.Errors.First().ErrorMessage.Should().Be("Field is not valid");
        exception.Which.Errors.First().PropertyName.Should().Be("CreateProfileRequest.University");
	}
}