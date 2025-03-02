using System.Net;
using Application.Conferences.JoinConference;
using Domain.Entities;
using Domain.Models.Conference;
using FluentAssertions;
using IntegrationTests.Base;
using IntegrationTests.Base.Helpers;
using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTests.Conference;

public class JoinConferenceCommandHandlerTest(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task ShouldJoinConferenceSuccessfully()
    {
        var affiliationId = Guid.NewGuid();
        var conference = new Domain.Entities.Conference
        {
            Name = "Test Conference",
            Description = "Test Description",
            StartDate = DateTime.UtcNow.AddDays(2),
            EndDate = DateTime.UtcNow.AddDays(3),
            ArticlesDeadline = DateTime.UtcNow.AddDays(1),
            RegistrationDeadline = DateTime.UtcNow.AddDays(1),
            Address = new Address
            {
                PlaceName = Faker.Company.CatchPhrase(),
                City = Faker.Address.USCity(),
                AddressLine1 = Faker.Address.StreetName(),
                AddressLine2 = Faker.Address.SecondaryAddress(),
                Country = "Poland",
                ZipCode = "11-111"
            },
            IsPublic = true
        };
        DbContext.Conferences.Add(conference);
        await DbContext.SaveChangesAsync();

        new IdentityBuilder()
            .WithUserName(Consts.UserName)
            .WithEmail(Consts.Email)
            .WithProfile(new User
            {
                Name = "Test User",
                Surname = "Tester",
                OrcidId = "0000-0002-1825-0097",
                IsProfileSetUp = true,
                Affiliations =
                [
                    new Affiliation
                    {
                        Id = affiliationId,
                        Workplace = "Test Workplace",
                        Position = "Researcher",
                        IsAcademic = true
                    }
                ]
            })
            .AddToDatabase(DbContext)
            .SetInHttpContextAccessor(HttpContextAccessor)
            .Build();

        var command = new JoinConferenceCommand(conference.Id, affiliationId);

        var result = await Mediator.Send(command);

        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.ErrorResultOptional.Should().BeNull();
        
        var attendeeCount = await DbContext.Attendees.CountAsync();
        attendeeCount.Should().Be(1);
    }
    
    [Fact]
    public async Task ShouldReturnUnauthorizedWhenUserNotAuthenticated()
    {
        var conference = new Domain.Entities.Conference
        {
            Name = "Test Conference",
            Description = "Test Description",
            StartDate = DateTime.UtcNow.AddDays(2),
            EndDate = DateTime.UtcNow.AddDays(3),
            ArticlesDeadline = DateTime.UtcNow.AddDays(1),
            RegistrationDeadline = DateTime.UtcNow.AddDays(1),
            Address = new Address
            {
                PlaceName = Faker.Company.CatchPhrase(),
                City = Faker.Address.USCity(),
                AddressLine1 = Faker.Address.StreetName(),
                AddressLine2 = Faker.Address.SecondaryAddress(),
                Country = "Poland",
                ZipCode = "11-111"
            },
            IsPublic = true
        };
        DbContext.Conferences.Add(conference);
        await DbContext.SaveChangesAsync();

        var command = new JoinConferenceCommand(conference.Id, Guid.NewGuid());

        var result = await Mediator.Send(command);

        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.ErrorResultOptional.Should().NotBeNull();
        result.ErrorResultOptional!.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task ShouldReturnBadRequestWhenUserAlreadyJoined()
    {
        var affiliationId = Guid.NewGuid();
        var conference = new Domain.Entities.Conference
        {
            Name = "Test Conference",
            Description = "Test Description",
            StartDate = DateTime.UtcNow.AddDays(2),
            EndDate = DateTime.UtcNow.AddDays(3),
            ArticlesDeadline = DateTime.UtcNow.AddDays(1),
            RegistrationDeadline = DateTime.UtcNow.AddDays(1),
            Address = new Address
            {
                PlaceName = Faker.Company.CatchPhrase(),
                City = Faker.Address.USCity(),
                AddressLine1 = Faker.Address.StreetName(),
                AddressLine2 = Faker.Address.SecondaryAddress(),
                Country = "Poland",
                ZipCode = "11-111"
            },
            IsPublic = true
        };
        DbContext.Conferences.Add(conference);
        await DbContext.SaveChangesAsync();

        new IdentityBuilder()
            .WithUserName(Consts.UserName)
            .WithEmail(Consts.Email)
            .WithProfile(new User
            {
                Name = "Test User",
                Surname = "Tester",
                OrcidId = "0000-0002-1825-0097",
                IsProfileSetUp = true,
                Affiliations =
                [
                    new Affiliation
                    {
                        Id = affiliationId,
                        Workplace = "Test Workplace",
                        Position = "Researcher",
                        IsAcademic = true
                    }
                ]
            })
            .AddToDatabase(DbContext)
            .SetInHttpContextAccessor(HttpContextAccessor)
            .Build();

        var command = new JoinConferenceCommand(conference.Id, affiliationId);

        var firstResult = await Mediator.Send(command);

        var secondResult = await Mediator.Send(command);

        firstResult.Should().NotBeNull();
        firstResult.IsSuccess.Should().BeTrue();

        secondResult.Should().NotBeNull();
        secondResult.IsSuccess.Should().BeFalse();
        secondResult.ErrorResultOptional.Should().NotBeNull();
        secondResult.ErrorResultOptional!.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}