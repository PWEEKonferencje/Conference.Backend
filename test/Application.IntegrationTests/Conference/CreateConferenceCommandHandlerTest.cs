using Application.Conferences.CreateConference;
using Domain.Entities;
using Domain.Models.Conference;
using Domain.ValueObjects;
using FluentAssertions;
using IntegrationTests.Base;
using IntegrationTests.Base.Helpers;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTests.Conference;

public class CreateConferenceCommandHandlerTest(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
	[Fact]
	public async Task ShouldCreateEntitiesAndReturn201()
	{
		//arrange
		var affiliationId = Guid.NewGuid();

		var command = new CreateConferenceCommand
		{
			Name = "Test Conference",
			Description = "Test Description",
			StartDate = DateTime.UtcNow.AddDays(2),
			EndDate = DateTime.UtcNow.AddDays(3),
			ArticlesDeadline = DateTime.UtcNow.AddDays(1),
			RegistrationDeadline = DateTime.UtcNow.AddDays(1),
			UserAffiliationId = affiliationId,
			Address = new AddressModel
			{
				PlaceName = Faker.Company.CatchPhrase(),
				City = Faker.Address.USCity(),
				AddressLine1 = Faker.Address.StreetName(),
				AddressLine2 = Faker.Address.SecondaryAddress(),
				Country = "Poland",
				ZipCode = "11-111"
			}
		};
		
		new IdentityBuilder()
			.WithUserName(Consts.UserName)
			.WithEmail(Consts.Email)
			.WithProfile(new User
			{
				Name = Faker.Name.FirstName(),
				Surname = Faker.Name.LastName(),
				OrcidId = Consts.Orcid,
				IsProfileSetUp = true,
				Affiliations =
				[
					new Affiliation
					{
						Id = affiliationId,
						Workplace = Faker.Company.Industry(),
						Position = Faker.Commerce.Department(),
						IsAcademic = true
					}
				]
			})
			.AddToDatabase(DbContext)
			.SetInHttpContextAccessor(HttpContextAccessor)
			.Build();
		
		//act
		var result = await Mediator.Send(command);
		
		//assert
		result.Should().NotBeNull();
		result.IsSuccess.Should().BeTrue();
		result.ErrorResultOptional.Should().BeNull();
		var conferenceCount = await DbContext.Conferences.CountAsync();
		conferenceCount.Should().Be(1);
		var attendeeCount = await DbContext.Attendees.CountAsync();
		attendeeCount.Should().Be(1);
		var snapsCount = await DbContext.UserSnapshots.CountAsync();
		snapsCount.Should().Be(1);
	}
}